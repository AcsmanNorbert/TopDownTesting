using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;

public class SpellCasting
{
    //----------------------IMPORTANT--------------------------
    //      layerMask should be ~ only in ...Overlap part
    //---------------------------------------------------------
    // and 3*/ is usefull :)

    public LayerMask ConvertLayerToLayerMask(int layer) => (1 << layer);

    #region COLLIDERS
    /// <summary>
    /// Explosion collision check with a defined sphere.
    /// </summary>
    /// <param name="transform">The damagers transform and the center of the sphere.</param>
    /// <param name="damage">The damage parameter and contains the sphere's radius.</param>
    /// <returns></returns>
    public static bool SphereBurstCollision(Transform transform, Damage damage)
        => BurstCollision(SphereOverlap(transform, damage.hitRadius, damage.layerMask), transform, damage,
            out List<Collider> colliders);

    /// <summary>
    /// Explosion collision check with a defined sphere and returns with a list of colliders.
    /// </summary>
    /// <param name="transform">The damagers transform and the center of the sphere.</param>
    /// <param name="damage">The damage parameter and contains the sphere's radius.</param>
    /// <param name="colliders">The list of colliders that were hit.</param>
    /// <returns></returns>
    public static bool SphereBurstCollision(Transform transform, Damage damage, out List<Collider> colliders)
        => BurstCollision(SphereOverlap(transform, damage.hitRadius, damage.layerMask), transform, damage, out colliders);

    /// <summary>
    /// Projectile collision check with a defined sphere.
    /// </summary>
    /// <param name="transform">The damagers transform and the center of the sphere.</param>
    /// <param name="damage">The damage parameter and contains the sphere's radius.</param>
    /// <returns></returns>
    public static bool SphereContinuousCollision(Transform transform, Damage damage)
        => ContinuousCollision(SphereOverlap(transform, damage.hitRadius, damage.layerMask), transform, damage, out List<Collider> colliders);

    /// <summary>
    /// Projectile collision check with a defined sphere and returns with a list of colliders.
    /// </summary>
    /// <param name="transform">The damagers transform and the center of the sphere.</param>
    /// <param name="damage">The damage parameter and contains the sphere's radius.</param>
    /// <param name="colliders">The list of colliders that were hit.</param>
    /// <returns></returns>
    public static bool SphereContinuousCollision(Transform transform, Damage damage, out List<Collider> colliders)
        => ContinuousCollision(SphereOverlap(transform, damage.hitRadius, damage.layerMask), transform, damage, out colliders);
    private static Collider[] SphereOverlap(Transform transform, float radius, LayerMask layerMask)
        => Physics.OverlapSphere(transform.position, radius, ~layerMask);

    /// <summary>
    /// Explosion collision check with a defined box.
    /// </summary>
    /// <param name="transform">The damagers transform and the center of the box.</param>
    /// <param name="damage">The damage parameter and contains the box's bounds.</param>
    /// <returns></returns>
    public static bool BoxBurstCollision(Transform transform, Damage damage)
        => BurstCollision(BoxOverlap(transform, damage.hitBounds, damage.layerMask), transform, damage, out List<Collider> colliders);

    /// <summary>
    /// Explosion collision check with a defined box and returns with a list of colliders.
    /// </summary>
    /// <param name="transform">The damagers transform and the center of the box.</param>
    /// <param name="damage">The damage parameter and contains the box's bounds.</param>
    /// <param name="colliders">The list of colliders that were hit.</param>
    /// <returns></returns>
    public static bool BoxBurstCollision(Transform transform, Damage damage, out List<Collider> colliders)
        => BurstCollision(BoxOverlap(transform, damage.hitBounds, damage.layerMask), transform, damage, out colliders);

    private static Collider[] BoxOverlap(Transform transform, Vector3 size, LayerMask layerMask)
        => Physics.OverlapBox(transform.position, size, transform.rotation, ~layerMask);
    #endregion

    /// <summary>
    /// Explosion collision check with defined collider
    /// </summary>
    /// <param name="hitColliders">The collider bounds.</param>
    /// <param name="transform">The damagers transform. (NOT USED YET)</param>
    /// <param name="damage">The damage parameter.</param>
    /// <param name="colliders">List of all hit colliders.</param>
    /// <returns></returns>
    public static bool BurstCollision(Collider[] explosionHit, Transform transform, Damage damage, out List<Collider> colliders)
    {
        colliders = new List<Collider>();
        foreach (var collider in explosionHit)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                colliders.Add(collider);
                if (damage.baseDamage > 0)
                    damageable.DoDamage(damage.baseDamage, damage.damageType, transform);                
                if (damage.fireDamage > 0)
                    FireDOT.CauseFire(collider.gameObject, damage.fireDamage);
                Debug.Log($"{collider.name} - {damage.baseDamage} dmg({damage.damageType})");
            }
        }
        if (colliders != null) return true;
        return false;
    }
    
    /// <summary>
    /// Projectile collision check with defined collider
    /// </summary>
    /// <param name="hitColliders">The collider bounds.</param>
    /// <param name="transform">The damagers transform. (NOT USED YET)</param>
    /// <param name="damage">The damage parameter.</param>
    /// <param name="colliders">List of all hit colliders.</param>
    /// <returns></returns>
    public static bool ContinuousCollision(Collider[] hitColliders, Transform transform, Damage damage, out List<Collider> colliders)
    {
        colliders = new List<Collider>();
        if (hitColliders == null) return false;

        foreach (var collider in hitColliders)
        {
            //13 = walls
            if (collider.gameObject.layer == 13)
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
                    if (damage.baseDamage > 0)
                        damageable.DoDamage(damage.baseDamage, damage.damageType, transform);
                    if (damage.fireDamage > 0)
                        FireDOT.CauseFire(collider.gameObject, damage.fireDamage);
                    Debug.Log($"{collider.name} - {damage.baseDamage} dmg({damage.damageType})");
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Shoots a prefab projectile from an objects position.
    /// </summary>
    /// <param name="projectilePrefab">The prefab that is being shot out.</param>
    /// <param name="shootingPosition">The transform the prefab getting shot from.</param>
    public static void ShootProjectile(GameObject projectilePrefab, Transform shootingPosition)
    {
        GameObject projectile = GameObject.Instantiate(projectilePrefab);
        if (projectile.activeSelf == false)
            projectile.SetActive(true);
        projectile.transform.position = shootingPosition.position;
        projectile.transform.rotation = shootingPosition.rotation;
    }
}

[Serializable]
public class Damage
{
    public LayerMask layerMask;
    public float baseDamage;
    public float hitRadius;
    public Vector3 hitBounds;
    [Space(3)]
    public float fireDamage;

    public enum DamageType
    {
        Direct,
        AreaOfEffect,
        DamageOverTime
    }

    public DamageType damageType;
}

public class FireDOT : MonoBehaviour
{
    float baseTickAmount = 5;
    float currentTickAmount;
    float currentDamage;

    bool isRunning;

    float tickRate = 0.4f;

    public void Refresh(float newDamage)
    {
        if (currentDamage < newDamage)
            currentDamage = newDamage;
        if (currentTickAmount < baseTickAmount)
            currentTickAmount = baseTickAmount;
        if (!isRunning)
            StartCoroutine(DoDoT());
    }

    private IEnumerator DoDoT()
    {
        isRunning = true;
        float damageOverTime = currentDamage / baseTickAmount;
        while (currentTickAmount > 0)
        {
            yield return new WaitForSeconds(tickRate);
            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.DoDamage(damageOverTime, Damage.DamageType.DamageOverTime, transform);
                //Debug.Log(dot + " DMG");
                currentTickAmount--;
            }
            else
                Destroy(this);
        }
        Destroy(this);
    }
    /// <summary>
    /// Makes a damagable object lit on fire and takes damage over time.
    /// </summary>
    /// <param name="target">The target that takes damage.</param>
    /// <param name="totalDamage">The total amount of damage that will be dealt to the target.</param>
    public static void CauseFire(GameObject target, float totalDamage)
    {
        FireDOT dot = target.GetComponent<FireDOT>();
        if (dot != null)
            dot.Refresh(totalDamage);
        else
            target.AddComponent<FireDOT>().Refresh(totalDamage);
    }
}

