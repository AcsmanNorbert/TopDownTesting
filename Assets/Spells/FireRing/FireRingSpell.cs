using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class FireRingSpell : MonoBehaviour
{
    [SerializeField] float castTime;
    [SerializeField] VisualEffect visualEffect;

    [Space(3)]
    [SerializeField] Damage damage;

    [Space(3)]
    public bool showGizmos;

    bool isPlaying;

    private void Start()
    {
        visualEffect.SetFloat("FlameDelay", castTime);
        visualEffect.SetFloat("FlameSize", damage.explosionRadius);
        visualEffect.Play();
        StartCoroutine(Lerp());
    }

    private void Update()
    {
        if (isPlaying)
            if (visualEffect.aliveParticleCount <= 0)
                Destroy(gameObject);
    }

    private IEnumerator Lerp()
    {
        yield return new WaitForSeconds(castTime);
        SpellCasting.SphereExplosion(transform, damage);
        yield return new WaitForSeconds(1f);
        isPlaying = true;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damage.explosionRadius);
        }
    }
}
