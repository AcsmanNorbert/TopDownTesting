using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CloseRangerAttack : MonoBehaviour
{
    [SerializeField] EnemyNavMesh enemyNavMesh;
    [Description("If in this range when the attack happens call OnCloseAttack, otherwise call OnRangesAttack")]
    [SerializeField] float closeRange = 2;

    [InspectorLabel("Ranged Attack")]
    [SerializeField] Damage rangedAttackDamage;
    [SerializeField] Transform rangedAttackPositionTransform;
    public UnityEvent OnRangedAttack = new UnityEvent();

    [InspectorLabel("Close Attack")]
    [SerializeField] Damage closeAttackDamage;
    [SerializeField] Transform closeAttackPositionTransform;
    public UnityEvent OnCloseAttack = new UnityEvent();

    public void StartAttack()
    {
        enemyNavMesh.InAttackRange(out float distance);

        if (distance > closeRange)
            OnRangedAttack?.Invoke();
        else
            OnCloseAttack?.Invoke();
    }

    public void DoRangedAttack()
    {
        SpellCasting.BoxBurstCollision(rangedAttackPositionTransform, rangedAttackDamage);
    }

    public void DoCloseAttack()
    {
        SpellCasting.SphereBurstCollision(closeAttackPositionTransform, closeAttackDamage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(closeAttackPositionTransform.position, closeAttackDamage.hitRadius);

        Gizmos.color = Color.magenta;
        Matrix4x4 gizmoMatrix = Matrix4x4.TRS(
            rangedAttackPositionTransform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = gizmoMatrix;
        Gizmos.DrawWireCube(Vector3.zero, rangedAttackDamage.hitBounds);
    }
}
