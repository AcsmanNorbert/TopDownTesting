using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Firewall : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;

    [Header("Data")]
    [SerializeField] Damage damage;
    [SerializeField][Range(0, 10)] float duration = 5;
    [SerializeField] float tickTimer = 0.2f;

    [Space(3)]
    [SerializeField] LayerMask projectileMask;
    [SerializeField] bool blockProjectile;

    [SerializeField] bool drawGizmos;
    bool isPlaying;

    List<Collider> hitColliders = new List<Collider>();

    private void Start()
    {
        visualEffect.SetFloat("Width", damage.hitBounds.x);
        visualEffect.SetFloat("Depth", damage.hitBounds.z);
        visualEffect.SetFloat("Duration", duration);

        StartCoroutine(Lerp());
    }

    private void Update()
    {
        if (isPlaying)
            if (visualEffect.aliveParticleCount == 0)
                Destroy(gameObject);
        /*
        if (SpellCasting.BoxBurstCollision(transform, damage, out List<Collider> colliders))
        {
            foreach (var collider in colliders)
            {
                bool wasHit = false;

                if (hitColliders != null)
                    foreach (var alreadyDamaged in hitColliders)
                        if (collider == alreadyDamaged)
                            wasHit = true;

                if (!wasHit)
                {
                    hitColliders.Add(collider);
                    StartCoroutine(DelayDMG(collider));

                    collider.GetComponent<IDamageable>().DoDamage(damage.baseDamage, Damage.DamageType.Direct, transform);
                }
            }
        }*/

        if (!blockProjectile) return;

        Collider[] projectileColliders = Physics.OverlapBox(
            transform.position, new Vector3(damage.hitBounds.x, 4, 0.5f), transform.rotation, projectileMask);
        if (projectileColliders != null)
        {
            foreach (var collider in projectileColliders)
            {
                Destroy(collider.gameObject);
            }
        }
    }

    private IEnumerator Lerp()
    {
        yield return new WaitForSeconds(1f);
        isPlaying = true;
    }

    private IEnumerator DelayDMG(Collider collider)
    {
        yield return new WaitForSeconds(tickTimer);
        hitColliders.Remove(collider);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {

            Matrix4x4 gizmoMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = gizmoMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(damage.hitBounds.x, 4, damage.hitBounds.z));

            if (blockProjectile)
            {
                Gizmos.matrix = gizmoMatrix;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(damage.hitBounds.x, 4, 0.5f));
            }
        }
    }
}
