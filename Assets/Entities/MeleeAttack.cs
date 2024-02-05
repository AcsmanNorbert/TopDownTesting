using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float meleeDamage;

    private void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (collider.gameObject.layer != gameObject.GetComponentInParent<Transform>().gameObject.layer)
            {
                damageable.DoDamage(meleeDamage, Damage.DamageType.Direct, transform);
            }
        }
    }
}
