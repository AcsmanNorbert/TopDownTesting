using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] Rigidbody rigibody;
    [SerializeField] float projectileSpeed;
    [SerializeField] Damage damage;

    [Space(3)]
    [SerializeField] bool drawGizmos = true;

    bool hasHit;

    private void Start()
    {
        rigibody.velocity = transform.forward * projectileSpeed;
    }

    private void Update()
    {
        if (!hasHit)
        {
            if (SpellCasting.SphereProjectile(transform, damage))
            {
                if (damage.explosionDamage > 0)
                    SpellCasting.SphereExplosion(transform, damage);
                SelfDestroy();
            }
        }
        else
            if (visualEffect.aliveParticleCount == 0)
                Destroy(gameObject);
    }

    public void SelfDestroy()
    {
        rigibody.velocity = new Vector3(0f, 0f, 0f);
        visualEffect.Stop();
        hasHit = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            Gizmos.DrawSphere(transform.position, damage.hitRadius);
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, damage.explosionRadius);
        }
    }
}
