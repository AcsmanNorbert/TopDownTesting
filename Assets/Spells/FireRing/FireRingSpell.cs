using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class FireRingSpell : MonoBehaviour
{
    [SerializeField] float castTime;
    [SerializeField] VisualEffect visualEffect;
    [Space(3)]

    [Header("Data")]
    [SerializeField] float explosionDamage;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask layerMask;

    [Space(5)]
    [Header("Burn Damage")]
    [SerializeField] bool causeFire;
    [SerializeField] float burnDamage;
    [SerializeField] int burnTicks;
    [SerializeField] float burnDuration;
    public bool showGizmos;

    bool isPlaying;

    private void Start()
    {
        StartCoroutine(Lerp());
    }

    private void Update()
    {
        if (isPlaying)
            if (visualEffect.aliveParticleCount <= 0)
                GameObject.Destroy(gameObject);
    }

    private IEnumerator Lerp()
    {
        visualEffect.SetFloat("FlameDelay", castTime);
        visualEffect.SetFloat("FlameSize", explosionRadius);
        visualEffect.Play();
        yield return new WaitForSeconds(castTime);
        Explosion();
        yield return new WaitForSeconds(1f);
        isPlaying = true;
    }

    private void Explosion()
    {
        Collider[] explosionHit = Physics.OverlapSphere(transform.position, explosionRadius, ~layerMask);
        foreach (var item in explosionHit)
        {
            IDamageable damageable2 = item.gameObject.GetComponent<IDamageable>();
            if (damageable2 != null)
            {
                damageable2.Damage(explosionDamage, IDamageable.DMGType.AreaOfEffect, transform);
                if (causeFire)
                    item.AddComponent<DOTDebuff>().DamageOverTime(
                        burnDamage, burnTicks, burnDuration, DOTDebuff.DOTType.Fire);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
