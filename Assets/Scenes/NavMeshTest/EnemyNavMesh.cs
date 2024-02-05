using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] GameObject mesh;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;

    [Space(10)]
    public bool showGizmos;
    [SerializeField] bool makeAISleep = false;

    [Header("Data")]
    [Tooltip("The time between two OnAttackTrigger events are called")]
    [SerializeField] float attackSpeed = 1f;
    [Tooltip("If false, wait for Attack Speed amount of time before it's first attack")]
    [SerializeField] bool attackImmediately = false;
    [Space(3)]
    [Tooltip("If true, finds player immdeiately")]
    [SerializeField] bool findTargetImmediately = false;
    [SerializeField] float seekingMovementSpeed = 1f;
    [SerializeField] float seekingAngularSpeed = 300f;
    [SerializeField] float seekingDistance = 10f;
    [Space(3)]
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
            Debug.Log("There was no mesh set to " + gameObject.name + ". I set " + mesh.name);
        }
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnValidate()
    {
        navMeshAgent.stoppingDistance = followDistance;
    }

    //Update inside STATE_MACHINE -> SETUP
    #region STATE_MACHINE

    #region SETUP
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
                canAttack = attackImmediately;
                break;
            default:
                break;
        }
        currentState = newState;
    }

    private void Update()
    {
        if (!makeAISleep)
        {
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
    }
    #endregion SETUP

    #region SEEKING
    Transform target;

    private void SeekingUpdate()
    {
        if (!findTargetImmediately)
        {
            navMeshAgent.speed = seekingMovementSpeed;
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

    public static float alertRadius = 8f;
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
    private void FollowUpdate()
    {
        navMeshAgent.speed = followMovementSpeed;
        navMeshAgent.angularSpeed = followAngularSpeed;
        navMeshAgent.destination = target.position;
        if (InAttackRange())
            SetState(State.Attack);
    }
    public bool InAttackRange() => target != null ? Vector3.Distance(transform.position, target.position) <= navMeshAgent.stoppingDistance : false;
    public bool InAttackRange(out float distance)
    {
        distance = navMeshAgent.stoppingDistance - navMeshAgent.remainingDistance;
        return target != null ? Vector3.Distance(transform.position, target.position) <= navMeshAgent.stoppingDistance : false;
    }
    #endregion FOLLOW
    
    #region ATTACK
    bool canAttack;
    float attackTimer;

    private void AttackUpdate()
    {
        transform.LookAt(target, Vector3.up);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        if (!canAttack)
        {
            if (attackTimer >= attackSpeed)
            {
                if (attackImmediately)
                {
                    if (!InAttackRange())
                        SetState(State.Follow);
                    else
                        canAttack = true;
                }
                else
                    canAttack = true;
            }
            else
                attackTimer += Time.deltaTime;
        }
        else
        {
            canAttack = false;
            attackTimer = 0f;
            OnAttackTrigger?.Invoke();
            if (!InAttackRange())
                SetState(State.Follow);
        }
    }
    #endregion ATTACK

    #region DEATH
    public static float corpseTimer = 15f;
    private IEnumerator Dies()
    {
        navMeshAgent.isStopped = true;

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
            if(alertGizmoDraw)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(alertPosition, alertRadius);
            }
        }
    }
}
