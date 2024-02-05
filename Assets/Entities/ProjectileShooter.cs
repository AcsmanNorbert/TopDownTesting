using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    
    public void ShootProjectile()
    {
        Instantiate(projectile);
        projectile.SetActive(true);
        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;
        projectile.GetComponent<Rigidbody>().velocity = projectileSpeed * projectile.transform.forward;
    }
}
