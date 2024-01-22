using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting
{
    //----------------------IMPORTANT--------------------------
    // layerMask should be ~ only in Collider[] reference part
    //---------------------------------------------------------
    // and 3*/ is usefull :)

    #region COLLIDERS
    public static bool SphereExplosion(Transform transform, Damage damage)
        => ExplosionCollision(SphereOverlap(transform, damage.explosionRadius, damage.layerMask), transform, damage,
            out List<Collider> colliders);
    public static bool SphereExplosion(Transform transform, Damage damage, out List<Collider> colliders)
        => ExplosionCollision(SphereOverlap(transform, damage.explosionRadius, damage.layerMask), transform, damage, out colliders);
    public static bool SphereProjectile(Transform transform, Damage damage)
        => ProjectileCollision(SphereOverlap(transform, damage.hitRadius, damage.layerMask), transform, damage, out List<Collider> colliders);
    public static bool SphereProjectile(Transform transform, Damage damage, out List<Collider> colliders)
        => ProjectileCollision(SphereOverlap(transform, damage.hitRadius, damage.layerMask), transform, damage, out colliders);
    private static Collider[] SphereOverlap(Transform transform, float radius, LayerMask layerMask)
        => Physics.OverlapSphere(transform.position, radius, ~layerMask);

    public static bool BoxExplosion(Transform transform, Vector3 size, Damage damage)
        => ExplosionCollision(BoxOverlap(transform, size, damage.layerMask), transform, damage, out List<Collider> colliders);
    public static bool BoxExplosion(Transform transform, Vector3 size, Damage damage, out List<Collider> colliders)
        => ExplosionCollision(BoxOverlap(transform, size, damage.layerMask), transform, damage, out colliders);
    private static Collider[] BoxOverlap(Transform transform, Vector3 size, LayerMask layerMask)
        => Physics.OverlapBox(transform.position, size, transform.rotation, ~layerMask);
    #endregion

    public static bool ExplosionCollision(Collider[] explosionHit, Transform transform, Damage damage, out List<Collider> colliders)
    {
        colliders = new List<Collider>();
        foreach (var collider in explosionHit)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                colliders.Add(collider);
                if (damage.explosionDamage > 0)
                {
                    damageable.Damage(damage.explosionDamage, Damage.DamageType.AreaOfEffect, transform);
                    //Debug.Log(damage.explosionDamage + " DMG to "+ collider.name);
                }
                if (damage.fireDamage > 0)
                    FireDOT.CauseFire(collider.gameObject, damage.fireDamage);
            }
        }
        if (colliders != null)
            return true;
        return false;
    }

    public static bool ProjectileCollision(Collider[] hitColliders, Transform transform, Damage damage, out List<Collider> colliders)
    {
        colliders = new List<Collider>();
        if (hitColliders != null)
        {
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject.layer == 0)
                {
                    colliders.Add(collider);
                    return true;
                }
                else
                {
                    IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        colliders.Add(collider);
                        if (damage.directDamage > 0)
                        {
                            damageable.Damage(damage.directDamage, Damage.DamageType.Direct, transform);
                            //Debug.Log("Direct damage: " + collider.name);
                        }
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

[Serializable]
public class Damage
{
    public LayerMask layerMask;
    public float directDamage;
    public float hitRadius;
    [Space(3)]
    public float explosionDamage;
    public float explosionRadius;
    [Space(3)]
    public float fireDamage;

    public enum DamageType
    {
        Direct,
        AreaOfEffect,
        DamageOverTime
    }
}

public class FireDOT : MonoBehaviour
{
    float baseTickAmount = 5;
    float currentTickAmount;
    float dot;

    bool isRunning;

    float tickRate = 0.4f;

    public void Refresh(float damage)
    {
        if (dot < damage)
            dot = damage;
        if (currentTickAmount < baseTickAmount)
            currentTickAmount = baseTickAmount;
        if (!isRunning)
            StartCoroutine(DoDoT());
    }

    private IEnumerator DoDoT()
    {
        isRunning = true;
        while (currentTickAmount > 0)
        {
            yield return new WaitForSeconds(tickRate);
            Debug.Log("asd");
            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(dot, Damage.DamageType.DamageOverTime, transform);
                //Debug.Log(dot + " DMG");
                currentTickAmount--;
            }
            else
                Destroy(this);
        }
        Destroy(this);
    }

    public static void CauseFire(GameObject target, float damage)
    {
        FireDOT dot = target.GetComponent<FireDOT>();
        if (dot != null)
            dot.Refresh(damage);
        else
            target.AddComponent<FireDOT>().Refresh(damage);
    }
}

