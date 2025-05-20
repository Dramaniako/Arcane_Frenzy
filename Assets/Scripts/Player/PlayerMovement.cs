using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rigidBody;
    public Transform cameraTransform; // drag your Camera here in the inspector
    private float pitch = 0f;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float sensitivity;
    public float accelerationTime = 0.1f;
    private Vector3 smoothMoveVelocity;

    private bool isGrounded = true;
    public float smoothTime = 0.05f;
    public float rayLength = 1f;
    public Vector3 rayDirection = Vector3.down + Vector3.right;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Ray();

        Jump();
    }

    void FixedUpdate()
    {
        PlayerMove();
    }

    void LateUpdate()
    {
        MouseFollow();
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
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = new Vector3(xAxis, 0f, zAxis).normalized;

        // Transform local input to world space direction
        Vector3 targetVelocity = transform.TransformDirection(moveInput) * moveSpeed;

        // Ground/air control adjustment
        float controlMultiplier = isGrounded ? 1f : 0.5f;
        targetVelocity *= controlMultiplier;

        // Smooth acceleration/deceleration
        Vector3 velocity = Vector3.SmoothDamp(
            rigidBody.linearVelocity,
            new Vector3(targetVelocity.x, rigidBody.linearVelocity.y, targetVelocity.z),
            ref smoothMoveVelocity,
            accelerationTime
        );

        // Stop immediately when input is zero
        if (moveInput == Vector3.zero)
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        rigidBody.linearVelocity = velocity;

    }

    void MouseFollow()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

        // Rotate player on Y axis
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera on X axis (pitch)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
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
