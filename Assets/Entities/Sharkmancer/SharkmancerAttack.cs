using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SharkmancerAttack : MonoBehaviour
{
    [SerializeField] GameObject summonPrefab;
    [SerializeField] int maxSummonCount = 5;
    [SerializeField] int attacksBeforeSummon = 9;
    [SerializeField] float summonBoundSize = 10;
    [Space(10)]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] List<Transform> shootingPositions; 
    EnemyNavMesh enemyNavMesh;

    List<EnemyHealth> currentSummons = new List<EnemyHealth>();
    int attacksLeftToUse;
    [Space(10)]
    public UnityEvent OnProjectileAttack = new UnityEvent();
    public UnityEvent OnSummonShark = new UnityEvent();

    private void Awake()
    {
        enemyNavMesh = GetComponent<EnemyNavMesh>();
    }

    private void Start()
    {
        attacksLeftToUse = 0;
    }

    public void DoAttack()
    {
        if (attacksLeftToUse <= 0 && SummonableCount() > 0)
            OnSummonShark?.Invoke();
        else
            OnProjectileAttack?.Invoke();
    }

    private float SummonableCount()
    {
        List<EnemyHealth> deadList = new List<EnemyHealth>();

        foreach (var item in currentSummons)
            if (item == null)
                deadList.Add(item);
        foreach (var item in deadList)
            currentSummons.Remove(item);

        return maxSummonCount - currentSummons.Count;
    }

    private void DoProjectileAttack(int position)
    {
        if (enemyNavMesh.currentState == EnemyNavMesh.State.Dead) return;
        attacksLeftToUse--;
        SpellCasting.ShootProjectile(projectilePrefab, shootingPositions[position]);
    }

    private void DoSummonAttack()
    {
        if (enemyNavMesh.currentState == EnemyNavMesh.State.Dead) return;

        Bounds summonBounds = new Bounds();
        summonBounds.size = new Vector3(summonBoundSize, 0, summonBoundSize);
        summonBounds.center = transform.position;

        attacksLeftToUse = attacksBeforeSummon;
        for (int i = 0; i < SummonableCount(); i++)
        {
            GameObject summon = Instantiate(summonPrefab);
            NavMeshAgent navMesh = summon.GetComponent<NavMeshAgent>();
            navMesh.enabled = false;

            EnemyHealth summonHealth = summon.GetComponent<EnemyHealth>();
            summonHealth.Money = 1;
            currentSummons.Add(summonHealth);

            Vector3 teleportLocation = new(
                Random.Range(summonBounds.min.x, summonBounds.max.x),
                Random.Range(summonBounds.min.y, summonBounds.max.y),
                Random.Range(summonBounds.min.z, summonBounds.max.z));
            summon.transform.position = teleportLocation;
            summon.transform.eulerAngles = transform.forward;

            navMesh.enabled = true;
            EnemyNavMesh enemyNavMesh = summon.GetComponent<EnemyNavMesh>();
            enemyNavMesh.findTargetImmediately = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 gizmoPosition = new Vector3(transform.position.x, 0.1f, transform.position.z);
        Gizmos.DrawWireCube(gizmoPosition, new Vector3(summonBoundSize, 0, summonBoundSize));
    }
}
