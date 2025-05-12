using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody rigidBody;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float sensitivity;
    [SerializeField] bool isGrounded;
    float xAxis, zAxis, mouseMovementX, mouseMovementY;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        PlayerMovement();

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

    void PlayerMovement()
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
}
