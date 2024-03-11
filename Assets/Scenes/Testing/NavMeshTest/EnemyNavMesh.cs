using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] GameObject mesh;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider myCollider;
    public Transform player;

    [Space(10)]
    public bool showGizmos;
    [SerializeField] bool makeAISleep = false;

    [Header("Data")]
    [Tooltip("The time between two OnAttackTrigger events are called")]
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float stopMovementDuringAttack = 0.5f;
    [Tooltip("If false, wait for Attack Speed amount of time before it's first attack")]
    [SerializeField] bool attackImmediately = false;
    [Tooltip("Call AttackUpdate during FollowUpdate")]
    [SerializeField] bool alwaysAttack = false;
    [Space(10)]
    [Tooltip("If true, finds player immdeiately")]
    public bool findTargetImmediately = false;
    [SerializeField] float seekingMovementSpeed = 1f;
    [SerializeField] float seekingAngularSpeed = 300f;
    [SerializeField] float seekingDistance = 10f;
    [Space(10)]
    [SerializeField] float followMovementSpeed = 3.5f;
    [SerializeField] float followAngularSpeed = 500f;
    [Tooltip("Sets up NavMeshAgent's StoppingDistance")]
    [SerializeField] float followDistance;

    [Space(10)]
    public UnityEvent OnAttackTrigger;

    private void Start()
    {
        if (mesh == null)
        {
            GameObject meshSearch = transform.GetChild(0).gameObject;
            if (meshSearch == null)
                meshSearch = gameObject;
            mesh = meshSearch;
            Debug.Log("There was no mesh set to " + gameObject.name + ". I set " + mesh.name + ". Your welcome");
        }
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameManager.i.player.transform;
    }

    private void OnValidate()
    {
        navMeshAgent.stoppingDistance = followDistance;
        if (attackImmediately)
            attackTimer = attackSpeed;
    }

    // Update inside STATE_MACHINE -> SETUP
    #region STATE_MACHINE

    #region SETUP
    // State machine Setup
    public enum State
    {
        Seeking,
        Follow,
        Attack,
        Dead
    }

    public State currentState { private set; get; } = State.Seeking;

    public void SetState(State newState)
    {
        switch (newState)
        {
            case State.Seeking:

                break;
            case State.Follow:

                break;
            case State.Dead:
                StartCoroutine(Dies());
                break;
            case State.Attack:
                doingAttack = attackImmediately;
                break;
            default:
                break;
        }
        currentState = newState;
    }

    private void Update()
    {
        if (!navMeshAgent.enabled) return;
        if (makeAISleep) return;

        switch (currentState)
        {
            case State.Seeking:
                SeekingUpdate();
                break;
            case State.Follow:
                FollowUpdate();
                break;
            case State.Attack:
                AttackUpdate();
                break;
        }
    }
    #endregion SETUP

    #region SEEKING
    // Seeking

    Transform target;

    private void SeekingUpdate()
    {
        if (!findTargetImmediately)
        {
            navMeshAgent.speed = seekingMovementSpeed;
            navMeshAgent.acceleration = seekingMovementSpeed;
            navMeshAgent.angularSpeed = seekingAngularSpeed;
            if (Vector3.Distance(transform.position, player.position) <= seekingDistance)
                TargetFoundAlert();
        }
        else
            TargetFoundAlert();
    }

    public void TargetFound()
    {
        if (currentState == State.Seeking)
        {
            target = player;
            SetState(State.Follow);
        }
    }

    public static float alertRadius = 16f;
    Vector3 alertPosition;
    bool alertGizmoDraw;
    float alertGizmoDuration = 1f;

    public void TargetFoundAlert()
    {
        alertPosition = transform.position;
        StartCoroutine(AlertGizmoShow());
        Collider[] alertColliders = Physics.OverlapSphere(transform.position, alertRadius);
        if (alertColliders != null)
        {
            foreach (Collider collider in alertColliders)
            {
                EnemyNavMesh enemyNavMesh = collider.GetComponent<EnemyNavMesh>();
                if (enemyNavMesh != null)
                    enemyNavMesh.TargetFound();
            }
        }
    }

    IEnumerator AlertGizmoShow()
    {
        alertGizmoDraw = true;
        yield return new WaitForSeconds(alertGizmoDuration);
        alertGizmoDraw = false;
    }
    #endregion SEEKING

    #region FOLLOW
    // Follow

    private void FollowUpdate()
    {
        navMeshAgent.speed = followMovementSpeed;
        navMeshAgent.acceleration = followMovementSpeed;
        navMeshAgent.angularSpeed = followAngularSpeed;
        navMeshAgent.destination = target.position;
        //here i stopped
        if (!alwaysAttack)
        {
            if (InAttackRange())
                SetState(State.Attack);
        }
        else
            AttackUpdate();
        if(attackImmediately)
            TickAttackTimer();
    }
    public bool InAttackRange() => target != null ? Vector3.Distance(transform.position, target.position) <= navMeshAgent.stoppingDistance : false;
    public bool InAttackRange(out float distance)
    {
        distance = Vector3.Distance(transform.position, target.position);
        return target != null ? Vector3.Distance(transform.position, target.position) <= navMeshAgent.stoppingDistance : false;
    }
    #endregion FOLLOW

    #region ATTACK
    // Attack

    bool doingAttack;
    float attackTimer;
    bool faceTarget = true;
    [SerializeField] float attackAngularSpeed;

    private void AttackUpdate()
    {
        if (faceTarget)
        {
            transform.LookAt(target, Vector3.up);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        //else
        // transform.eulerAngles = new Vector3(0, Vector3.RotateTowards(transform.position, target.position, attackAngularSpeed, float.MaxValue).y, 0);

        if (doingAttack) return;

        if (attackTimer >= attackSpeed)
        {
            if (currentState == State.Dead) return;
            if (!alwaysAttack && !InAttackRange())
                SetState(State.Follow);
            else
                StartCoroutine(StartAttack());
        }
        else
            TickAttackTimer();
    }

    private void TickAttackTimer() => attackTimer = attackTimer < attackSpeed ? attackTimer + Time.deltaTime : attackTimer;

    IEnumerator StartAttack()
    {
        doingAttack = true;
        OnAttackTrigger?.Invoke();

        yield return new WaitForSeconds(stopMovementDuringAttack);

        if (currentState != State.Dead)
        {
            faceTarget = true;
            if (!InAttackRange() && !alwaysAttack)
                SetState(State.Follow);
            attackTimer = 0f;
            doingAttack = false;
        }
    }
    #endregion ATTACK

    public void StartFacingTarget() => FaceTarget(true);
    public void StopFacingTarget() => FaceTarget(false);

    public void FaceTarget(bool doFace) => faceTarget = doFace;

    #region DEATH
    // Death

    public static float corpseTimer = 8f;
    public static float upwardsYippi = 8f;
    public static float sidewaysYippi = 5f;
    private IEnumerator Dies()
    {
        navMeshAgent.enabled = false;
        myCollider.isTrigger = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        Vector3 randomOffset = new Vector3(Random.Range(-sidewaysYippi, sidewaysYippi), upwardsYippi, Random.Range(-sidewaysYippi, sidewaysYippi));
        rb.velocity += randomOffset;

        yield return new WaitForSeconds(corpseTimer);
        Destroy(gameObject);
    }
    #endregion DEATH

    #endregion STATE_MACHINE

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if (currentState == State.Seeking)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, seekingDistance);
            }
            else if (currentState != State.Dead)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target.position);
                if (currentState == State.Follow)
                    Gizmos.color = Color.cyan;
                if (currentState == State.Attack)
                    Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, navMeshAgent.stoppingDistance);
            }
            if (alertGizmoDraw)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(alertPosition, alertRadius);
            }
        }
    }
}
