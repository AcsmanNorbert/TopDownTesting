using System.Collections;
using UnityEngine;

public class IceBomber : MonoBehaviour
{
    [SerializeField] EnemyNavMesh navMeshAgent;

    [Space(3)]
    [Header("Data")]
    [SerializeField] float damage;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionDelay;
    [SerializeField] LayerMask layerMask;

    public bool showGizmos;

    public void StartExplosion()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);

        if(navMeshAgent.currentState != EnemyNavMesh.State.Dead)
        {
            Collider[] explosionHit = Physics.OverlapSphere(transform.position, explosionRadius, ~layerMask);
            foreach (var item in explosionHit)
            {
                IDamageable damageable = item.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Debug.Log(item.gameObject.name);
                    damageable.DoDamage(damage, Damage.DamageType.AreaOfEffect, transform);
                }
            }
            Destroy(gameObject);
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
