using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] Rigidbody rigibody;

    [Space(10)]
    [SerializeField] bool drawGizmos = true;

    [Space(10)]
    [Header("Projectile Values")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] float projectileSpeed;
    [SerializeField] float directDamage;
    [SerializeField] float hitRadius;

    [Space(3)]
    [SerializeField] bool causeExplosion;
    [SerializeField] float explosionDamage;
    [SerializeField] float explosionRadius;

    [Space(3)]
    [SerializeField] bool causeFire;
    [SerializeField] float burnDamage;
    [SerializeField] int burnTicks;
    [SerializeField] float burnDuration;

    bool hasHit;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
    }

    private void Update()
    {
        if (!hasHit)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, hitRadius, ~layerMask);
            if (colliders != null)
            {
                foreach (var collider in colliders)
                {
                    IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
                    if (collider.gameObject.layer == 0)
                        SelfDestroy();

                    else if (damageable != null)
                    {
                        damageable.Damage(directDamage, IDamageable.DMGType.Direct, transform);
                        Debug.Log("Direct damage: " + collider.name);

                        #region EXPLOSION_DAMAGE
                        if (causeExplosion)
                        {
                            Collider[] explosionHit = Physics.OverlapSphere(transform.position, explosionRadius, ~layerMask);
                            foreach (var item in explosionHit)
                            {
                                IDamageable damageable2 = item.gameObject.GetComponent<IDamageable>();
                                if (damageable2 != null)
                                {
                                    damageable2.Damage(explosionDamage, IDamageable.DMGType.AreaOfEffect, transform);
                                    Debug.Log("Explosion damage: " + item.name);
                                    if (causeFire)
                                        item.AddComponent<DOTDebuff>().DamageOverTime(
                                            burnDamage, burnTicks, burnDuration, DOTDebuff.DOTType.Fire);
                                }
                            }
                        }
                        #endregion

                        SelfDestroy();
                    }
                }
            }
        }
        else
            if (visualEffect.aliveParticleCount == 0)
            Destroy(gameObject);
    }

    #region DESTROY_SELF

    public void SelfDestroy()
    {
        rigibody.velocity = new Vector3(0f, 0f, 0f);
        visualEffect.Stop();
        hasHit = true;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            Gizmos.DrawSphere(transform.position, hitRadius);
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, explosionRadius);
        }
    }
}

public class DOTDebuff : MonoBehaviour
{
    public enum DOTType
    {
        Fire,
        Poison
    }

    public DOTType dotType;

    public void DamageOverTime(float damage, int tickAmount, float time, DOTType type)
    {
        dotType = type;
        float damagePerTick = damage / tickAmount;
        float tickTime = time / tickAmount;

        StartCoroutine(TickDamage(damagePerTick, tickTime, tickAmount));
    }

    private IEnumerator TickDamage(float damagePerTick, float tickTime, float tickAmount)
    {
        for (int i = 0; i < tickAmount; i++)
        {
            yield return new WaitForSeconds(tickTime);
            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damagePerTick, IDamageable.DMGType.DamageOverTime, transform);
                Debug.Log((i + 1) + ". " + damagePerTick + " DMG");
            }
            else
                GameObject.Destroy(this);
        }
        GameObject.Destroy(this);
    }

    public static void CauseFire(GameObject target, float damage, int tickAmount, float duration, DOTType type)
    {
        DOTDebuff[] dot = target.GetComponents<DOTDebuff>();
        //if it has the same type destroy and apply the new one
        //could change it to just refresh duration
        bool hasIt = false;
        if (dot != null)
            foreach (var item in dot)
                if (item.dotType == type)
                    Destroy(item);
        if(!hasIt)
            target.AddComponent<DOTDebuff>().DamageOverTime(damage, tickAmount, duration, type);
    }
}
