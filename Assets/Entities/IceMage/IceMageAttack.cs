using UnityEngine;

public class IceMageAttack : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootingPosition;

    public void ShootProjectile()
    {
        if (GetComponent<EnemyNavMesh>().currentState == EnemyNavMesh.State.Dead) return;

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = shootingPosition.position;
        projectile.transform.rotation = shootingPosition.rotation;
    }
}
