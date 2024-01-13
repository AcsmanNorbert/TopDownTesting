using UnityEngine;

public class DamageData
{
    [Header("Damage Data")]
    public LayerMask layerMask;
    public float damage;

    [Space(3)]
    [Header("Burn Damage")]
    [SerializeField] bool causeFire;
    [SerializeField] float burnDamage;
    [SerializeField] int burnTicks;
    [SerializeField] float burnDuration;
}
