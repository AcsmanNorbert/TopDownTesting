using UnityEngine;

public class JawAttack : MonoBehaviour
{
    [SerializeField] Damage damage;
    [SerializeField] Transform attackPositionTransform;

    public void DoJawAttack()
    {
        SpellCasting.SphereBurstCollision(attackPositionTransform, damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPositionTransform.position, damage.hitRadius);
    }
}
