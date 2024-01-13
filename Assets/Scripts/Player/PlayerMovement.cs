using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    public GameObject mesh;
    [SerializeField] private Transform tailTarget;
    private Animator animator;

    #region MOVEMENT-ROTATION_INPUT
    private float horizontalMovement;
    private float verticalMovement;
    [SerializeField] private float speed = 5f;
    //[SerializeField] private float rotationSpeed = 15f;
    #endregion

    void Start()
    {
        animator = mesh.GetComponent<Animator>();
    }

    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        #region MOVEMENT
        Vector3 inputMovement = new Vector3(horizontalMovement, 0, verticalMovement);
        inputMovement = Quaternion.Euler(0, Camera.main.gameObject.transform.eulerAngles.y, 0) * inputMovement;
        transform.position += inputMovement * (speed * Time.deltaTime);

        //animation
        //float moveDistance = Vector3.Distance(inputMovement, mesh.transform.position);
        animator.SetFloat("walkingSpeed", Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement));

        //tail
        Vector3 tailPos = tailTarget.parent.position - inputMovement * 0.2f;
        tailTarget.position = Vector3.Lerp(tailTarget.position, tailPos, Time.deltaTime * 10f);
        #endregion

        #region ROTATE
        Vector3 target = PlayerInput.GetMousePosition(mesh.transform);
        target.y = transform.position.y;
        mesh.transform.LookAt(target);
        #endregion
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
