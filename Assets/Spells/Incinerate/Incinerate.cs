using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Incinerate : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] Damage damage;
    [SerializeField] bool drawGizmos;

    void Start()
    {
        SpellCasting.SphereBurstCollision(transform, damage);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(1f, 0f, 0f);
            Gizmos.DrawWireSphere(transform.position, damage.hitRadius);
        }
    }
}
