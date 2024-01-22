using UnityEngine;

public interface IDamageable
{
    void Damage(float damage, Damage.DamageType damageType, Transform transform);
}
