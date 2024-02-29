using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Damage damage;
    [SerializeField] Transform hitPosition;
    [SerializeField] float maxDistance = 10;
    [SerializeField] float maxLifeTime = 10;
    [SerializeField] float speed = 10;
    Vector3 startingPosition;
    float lifeTime = 0;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        startingPosition = transform.position;
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        SpellCasting.SphereContinuousCollision(hitPosition, damage, out List<Collider> hitColliders);
        if (Vector3.Distance(startingPosition, transform.position) >= maxDistance || lifeTime >= maxLifeTime || hitColliders.Count > 0)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(hitPosition.position, damage.hitRadius);
    }
}
