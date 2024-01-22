using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class IceBomber : MonoBehaviour
{
    [SerializeField] MeleeEnemy meleeEnemy;

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

        if(meleeEnemy.currentState != IMovementAI.State.Dead)
        {
            Collider[] explosionHit = Physics.OverlapSphere(transform.position, explosionRadius, ~layerMask);
            foreach (var item in explosionHit)
            {
                IDamageable damageable = item.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Debug.Log(item.gameObject.name);
                    damageable.Damage(damage, Damage.DamageType.AreaOfEffect, transform);
                }
            }
            GameObject.Destroy(gameObject);
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
