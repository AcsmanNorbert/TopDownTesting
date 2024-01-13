using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public GameObject prefab;
    public float baseCooldown;

    public int castPosition;

    public enum CastType
    {
        Target,
        Projectile,
        Cursor,
        Melee
    }
    public CastType castType;
}
