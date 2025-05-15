using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rigidBody;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float sensitivity;
    [SerializeField] bool isGrounded;
    public float rayLength = 1f;
    public Vector3 rayDirection = Vector3.down + Vector3.right;
    float xAxis, zAxis, mouseMovementX, mouseMovementY;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        Ray();

        PlayerMove();

        MouseFollow();

        Jump();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enter collision");
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("exit collision");
        isGrounded = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    void PlayerMove()
    {
        xAxis = Input.GetAxis("Horizontal") * moveSpeed;
        zAxis = Input.GetAxis("Vertical") * moveSpeed;

        Vector3 moveInput = new Vector3(xAxis, 0f, zAxis).normalized;

        Vector3 moveDirection = transform.TransformDirection(moveInput);

        float controlMultiplier = isGrounded ? 1f : 0.5f;

        Vector3 velocity = moveDirection * moveSpeed * controlMultiplier;
        velocity.y = rigidBody.linearVelocity.y;

        rigidBody.linearVelocity = velocity;
    }

    void MouseFollow()
    {
        mouseMovementX = Input.GetAxis("Mouse X") * sensitivity;
        gameObject.transform.Rotate(0f, mouseMovementX, 0f);
    }

    void Ray()
    {
        Ray ray = new Ray(transform.position, rayDirection.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.DrawRay(transform.position, rayDirection.normalized * rayLength, Color.red);
            isGrounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position, rayDirection.normalized * rayLength, Color.green);
        }

    }
}
