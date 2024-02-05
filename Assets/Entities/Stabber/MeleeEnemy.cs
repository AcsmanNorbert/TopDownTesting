/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class MeleeEnemy : MonoBehaviour, IMovementAI
{
    [SerializeField] private GameObject mesh;
    private Rigidbody rb;
    public Transform player;
    private Transform target;

    [Space(3)]
    public bool showGizmos;

    public IMovementAI.State currentState;

    [Space(3)]
    [Header("Data")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float seekingRadius;

    [Space(3)]
    [Header("Melee")]
    private float attackTimer;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float meleeDistance;
    private bool canAttack;

    [Space(3)]
    public UnityEvent OnAttackTrigger;

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
            SetState(IMovementAI.State.Follow);
        }
    }

    private void FollowUpdate()
    {
        if (attackTimer >= attackSpeed)
            canAttack = true;
        else
            attackTimer += Time.deltaTime;

        transform.LookAt(target.position, transform.up);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= meleeDistance)
        {
            if (canAttack)
            {
                canAttack = false;
                attackTimer = 0f;
                Attack();
            }
        }
        else
            rb.velocity = transform.forward * movementSpeed;
    }

    public void SetState(IMovementAI.State newState)
    {
        currentState = newState;
        switch (newState)
        {
            case IMovementAI.State.Seeking:
                break;
            case IMovementAI.State.Follow:
                canAttack = true;
                break;
            case IMovementAI.State.Dead:
                StartCoroutine(Dies());
                break;
            default:
                break;
        }
    }
    #endregion


    private void Attack()
    {
        OnAttackTrigger?.Invoke();
        //OnAttackTrigger?.Invoke(this, EventArgs.Empty);
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
                Gizmos.DrawWireSphere(transform.position, meleeDistance);
            }
        }
    }
}
*/