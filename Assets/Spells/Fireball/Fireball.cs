using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] GameObject explosion;
    [SerializeField] Rigidbody rigibody;
    [SerializeField] float projectileSpeed;
    [Space(10)]
    [SerializeField] Damage directDamage;
    [SerializeField] Damage explosionDamage;

    [Space(3)]
    public bool drawGizmos = true;

    private void Start()
    {
        rigibody.velocity = transform.forward * projectileSpeed;
    }

    private void Update()
    {
        if (SpellCasting.SphereContinuousCollision(transform, directDamage, out List<Collider> colliders))
        {
            SpellCasting.SphereBurstCollision(transform, explosionDamage);
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        rigibody.velocity = new Vector3(0f, 0f, 0f);
        visualEffect.Stop();
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            Gizmos.DrawSphere(transform.position, directDamage.hitRadius);
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, directDamage.hitRadius);
        }
    }
}
