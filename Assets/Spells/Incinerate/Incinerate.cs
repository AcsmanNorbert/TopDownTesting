using System.Collections;
using UnityEngine;

public class Incinerate : MonoBehaviour
{
    //[SerializeField] VisualEffect visualEffect;
    [SerializeField] LayerMask layerMask;

    [Space(3)]
    [Header("Data")]
    [SerializeField] float damage;
    [SerializeField] float explosionRadius;

    [Space(3)]
    [Header("Fire Damage")]
    [SerializeField] bool causeFire;
    [SerializeField] float burnDamage;
    [SerializeField] int burnTicks;
    [SerializeField] float burnDuration;

    bool isPlaying;

    void Start()
    {
        StartCoroutine(DoAttack());
        if (isPlaying)
            //if (visualEffect.aliveParticleCount == 0)
                Destroy(gameObject);
    }

    private IEnumerator DoAttack()
    {
        Collider[] explosionHit = Physics.OverlapSphere(transform.position, explosionRadius, ~layerMask);
        foreach (var collider in explosionHit)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damage, IDamageable.DMGType.AreaOfEffect, transform);
                if (causeFire)
                    DOTDebuff.CauseFire(
                        collider.gameObject, burnDamage, burnTicks, burnDuration, DOTDebuff.DOTType.Fire);
            }
        }

        yield return new WaitForSeconds(1f);

        isPlaying = true;
    }
}
