using UnityEngine;
using UnityEngine.VFX;

public class IceBomber : MonoBehaviour
{
    [SerializeField] EnemyNavMesh navMeshAgent;
    [SerializeField] Damage damage;
    [SerializeField] GameObject explosionPrefab;
    VisualEffect visualEffect;
    [SerializeField] bool hasRing;
    [SerializeField] float lifeTime = 1;

    public bool showGizmos;

    public void DoExplosion()
    {
        SpellCasting.SphereBurstCollision(transform, damage);

        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        visualEffect = explosion.GetComponent<VisualEffect>();

        visualEffect.SetFloat("Size", damage.hitRadius);
        visualEffect.SetFloat("LifeTime", lifeTime);
        int hasRingInt = hasRing == false ? 0 : 1;
        visualEffect.SetInt("HasRing", hasRingInt);

        visualEffect.Play();

        Destroy(gameObject);        
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, damage.hitRadius);
        }
    }
}
