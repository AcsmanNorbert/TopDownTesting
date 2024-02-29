using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SharkmancerAttack : MonoBehaviour
{
    [SerializeField] GameObject summonPrefab;
    [SerializeField] int maxSummonCount = 5;
    [SerializeField][FormerlySerializedAs("boundSize")] float summonBoundSize = 10;
    [Space(10)]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootingPosition;

    Bounds summonBounds;
    List<EnemyHealth> currentSummons = new List<EnemyHealth>();
    static float attacksBeforeSummon = 3;
    float attacksLeftToUse;

    public UnityEvent OnProjectileAttack = new UnityEvent();

    private void OnValidate()
    {
        summonBounds.size = new Vector3(summonBoundSize, 0, summonBoundSize);
    }

    public void DoAttack()
    {
        summonBounds.center = transform.position;
        List<EnemyHealth> deadList = new List<EnemyHealth>();

        foreach (var item in currentSummons)
            if (item == null)
                deadList.Add(item);
        foreach (var item in deadList)
            currentSummons.Remove(item);

        float missingCount = maxSummonCount - currentSummons.Count;

        if (attacksLeftToUse <= 0 && missingCount <= maxSummonCount)
            DoSummon(missingCount);
        else
            DoProjectileAttack();
    }

    private void DoProjectileAttack()
    {
        attacksLeftToUse--;
        OnProjectileAttack?.Invoke();
        SpellCasting.ShootProjectile(projectilePrefab, shootingPosition);
    }

    private void DoSummon(float summonCount)
    {
        attacksLeftToUse = attacksBeforeSummon;
        for (int i = 0; i < summonCount; i++)
        {
            GameObject summon = Instantiate(summonPrefab);
            currentSummons.Add(summon.GetComponent<EnemyHealth>());

            Vector3 teleportLocation = new(
                Random.Range(summonBounds.min.x, summonBounds.max.x),
                Random.Range(summonBounds.min.y, summonBounds.max.y),
                Random.Range(summonBounds.min.z, summonBounds.max.z));
            summon.transform.position = teleportLocation;
            summon.transform.eulerAngles = transform.forward;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 gizmoPosition = new Vector3(transform.position.x, 0.1f, transform.position.z);
        Gizmos.DrawWireCube(gizmoPosition, summonBounds.size);
    }
}
