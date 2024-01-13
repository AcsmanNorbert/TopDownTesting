using System.Diagnostics;
using UnityEngine;

public interface IDamageable
{
    enum DMGType
    { 
        Direct,
        AreaOfEffect,
        DamageOverTime
    }

    void Damage(float damage, DMGType dmgType, Transform transform);
}
