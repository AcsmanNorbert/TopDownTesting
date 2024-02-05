using UnityEngine;

public interface IDamageable
{
    void DoDamage(float damage, Damage.DamageType damageType, Transform transform);
}
