using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Incinerate : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] Damage damage;
    [SerializeField] bool drawGizmos;

    bool isPlaying;

    void Start()
    {
        StartCoroutine(DoAttack());
    }

    private void Update()
    {
        if (isPlaying)
            if (visualEffect.aliveParticleCount == 0)
            Destroy(gameObject);
    }

    private IEnumerator DoAttack()
    {
        SpellCasting.SphereExplosion(transform, damage);

        yield return new WaitForSeconds(1f);
        isPlaying = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(1f, 0f, 0f);
            Gizmos.DrawWireSphere(transform.position, damage.explosionRadius);
        }
    }
}
