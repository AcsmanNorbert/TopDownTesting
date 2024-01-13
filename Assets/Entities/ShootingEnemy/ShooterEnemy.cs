using System;
using System.Collections;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour, IMovementAI
{
    [SerializeField] GameObject mesh;
    [SerializeField] Transform shootingPosition;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject iceball;
    Rigidbody rb;
    public Transform player;
    Transform target;

    [Space(3)]
    public bool showGizmos;

    public IMovementAI.State currentState;

    [Space(3)]
    [Header("Data")]
    [SerializeField] float movementSpeed;
    [SerializeField] float seekingRadius;
    [SerializeField] float projectileSpeed;
    float moveTimer;

    [Space(3)]
    [Header("Shooting")]
    [SerializeField] float timeBetweenShots;
    [SerializeField] float followDistance;
    [SerializeField] float retreatDistance;
    bool canShoot;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    #region STATE_MACHINE

    private void Update()
    {
        switch (currentState)
        {
            case IMovementAI.State.Seeking:
                SeekingUpdate();
                break;
            case IMovementAI.State.Follow:
                FollowUpdate();
                break;
        }
    }

    private void SeekingUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) <= seekingRadius)
        {
            target = player;
            currentState = IMovementAI.State.Follow;
        }
    }

    private void FollowUpdate()
    {
        if (moveTimer >= timeBetweenShots)
            canShoot = true;
        else
            moveTimer += Time.deltaTime;

        mesh.transform.LookAt(target.position, mesh.transform.up);
        mesh.transform.eulerAngles = new Vector3(0f, mesh.transform.eulerAngles.y, 0f);

        if (canShoot)
        {
            canShoot = false;
            moveTimer = 0f;
            ShootProjectile();
        }
        else
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= followDistance)
            {
                if (distanceToTarget < retreatDistance)
                {
                    rb.velocity = -mesh.transform.forward * movementSpeed;
                }
            }
            else
                rb.velocity = mesh.transform.forward * movementSpeed;
        }
    }

    public void SetState(IMovementAI.State newState)
    {
        currentState = newState;
        switch (newState)
        {
            case IMovementAI.State.Seeking:
                break;
            case IMovementAI.State.Follow:
                break;
            case IMovementAI.State.Dead:
                StartCoroutine(Dies());
                break;
            default:
                break;
        }
    }

    #endregion
    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(iceball);
        projectile.transform.rotation = mesh.transform.rotation;
        projectile.transform.position = shootingPosition.position;
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
    }

    public void ApplyForce(Transform damager)
    {
        rb.velocity += damager.up * 10f;
    }

    private IEnumerator Dies()
    {
        rb.freezeRotation = false;
        rb.velocity += transform.forward;

        yield return new WaitForSeconds(15f);
        GameObject.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if (currentState == IMovementAI.State.Seeking)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, seekingRadius);
            }
            else if (currentState == IMovementAI.State.Follow)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target.gameObject.transform.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, followDistance);
            }
        }
    }
}
