using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject mesh;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform tailTarget;
    [SerializeField] Animator animator;

    [SerializeField] float speed = 4f;
    [SerializeField] float currentSpeed;
    float horizontalMovement;
    float verticalMovement;
    //[SerializeField] private float rotationSpeed = 15f;

    [Space(3)]
    public bool canDash = true;
    [SerializeField] float dashSpeed = 1.5f;
    [SerializeField] float dashTimer = 0.2f;
    public float dashCooldown = 1;
    public float dashCurrentCooldown { private set; get; }
    public static bool isDashing { private set; get; }

    void Start()
    {
        if (animator == null)
            animator = mesh.GetComponent<Animator>();
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.isPaused) return;

        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        currentSpeed = speed;

        if (dashCurrentCooldown > 0)
            dashCurrentCooldown -= Time.deltaTime;

        if (!canDash) return;
        if (Input.GetKeyDown(KeyCode.Space))
            if (dashCurrentCooldown <= 0)
                StartCoroutine(DashDuration());
    }

    private IEnumerator DashDuration()
    {
        dashCurrentCooldown = dashCooldown;
        if (rb.velocity == Vector3.zero)
            rb.velocity = mesh.transform.forward * currentSpeed * dashSpeed;
        else rb.velocity = InputMovement().normalized * currentSpeed * dashSpeed;

        isDashing = true;
        yield return new WaitForSeconds(dashTimer);
        isDashing = false;
    }

    private void FixedUpdate()
    {
        if (GameManager.isPaused) return;

        #region MOVEMENT
        Vector3 inputMovement = InputMovement();
        if (!isDashing)
            rb.velocity = inputMovement;

        //float moveDistance = Vector3.Distance(inputMovement, mesh.transform.position);
        animator.SetFloat("walkingSpeed", Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement));

        //tail
        Vector3 tailPos = tailTarget.parent.position - inputMovement / 10;
        tailTarget.position = Vector3.Lerp(tailTarget.position, tailPos, Time.deltaTime * 10f);
        #endregion

        #region ROTATE
        Vector3 target = PlayerInput.GetMousePosition(mesh.transform);
        target.y = transform.position.y;
        mesh.transform.LookAt(target);
        #endregion
    }

    private Vector3 InputMovement()
    {
        //the current input
        Vector3 inputMovement = new Vector3(horizontalMovement, 0, verticalMovement);
        //rotates the current input based on camera location
        inputMovement = Quaternion.Euler(0, Camera.main.gameObject.transform.eulerAngles.y, 0) * inputMovement;
        //magnifies the movement by its speed multiplyer and makes Y unchanged
        inputMovement = new Vector3(inputMovement.x * currentSpeed, rb.velocity.y, inputMovement.z * currentSpeed);
        return inputMovement;
    }

    private void OnDrawGizmos()
    {
        #region TESTING GETMOUSE POSITION
        /*Vector3 target = PlayerInput.GetMousePosition(mesh.transform); 
        target.y = transform.position.y;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 1f);*/
        #endregion
    }
}
