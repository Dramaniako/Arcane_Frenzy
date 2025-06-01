using UnityEngine;
using UnityEngine.EventSystems; // Required for checking if touch is over UI elements

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rigidBody;
    public Transform cameraTransform; // Drag your Camera here in the inspector

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float accelerationTime = 0.1f; // How quickly the player reaches full speed
    [SerializeField] float airControlMultiplier = 0.5f; // How much control player has in air

    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = 8f;

    [Header("Look Settings")]
    [SerializeField] float pcMouseSensitivity = 200f;
    [SerializeField] float mobileLookSensitivity = 2.5f;
    private float pitch = 0f; // Current vertical rotation of the camera

    [Header("Ground Check")]
    [SerializeField] float groundCheckRayLength = 0.6f; // Ray length from player pivot + verticalOffset
    [SerializeField] Vector3 groundCheckVerticalOffset = new Vector3(0, 0.1f, 0); // Offset ray start slightly above pivot base
    [SerializeField] LayerMask groundLayer; // Set this in inspector to your ground layer(s)
    private bool isGrounded = true;

    // Internal State
    private Vector3 smoothMoveVelocity; // Used by SmoothDamp for movement
    private Vector2 currentMovementInput; // (x, z) from either keyboard or virtual joystick
    private Vector2 currentLookInput;   // (mouseX, mouseY) from either mouse or touch drag

    // Mobile Touch State
    private int movementTouchId = -1;
    private Vector2 movementTouchStartPosition;
    private Rect movementTouchRect; // Left half of the screen for movement

    private int lookTouchId = -1;
    private Rect lookTouchRect; // Right half of the screen for looking

    void Start()
    {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
        if (cameraTransform == null) Debug.LogError("Camera Transform not assigned to PlayerMovement script!");

#if UNITY_EDITOR || UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif

        // Define touch areas for mobile
        movementTouchRect = new Rect(0, 0, Screen.width / 2, Screen.height);
        lookTouchRect = new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height);
    }

    void Update()
    {
        HandleInput();
        PerformGroundCheck(); // Raycast ground check is more reliable in Update/FixedUpdate

#if UNITY_EDITOR || UNITY_STANDALONE
        // PC Jump Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProcessJump();
        }
#endif
        // Note: Mobile jump will be handled by a UI Button calling PublicJump()
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void LateUpdate()
    {
        ApplyCameraLook();
    }

    void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // PC Input
        currentMovementInput.x = Input.GetAxisRaw("Horizontal");
        currentMovementInput.y = Input.GetAxisRaw("Vertical"); // Using y for z-axis movement (forward/backward)

        currentLookInput.x = Input.GetAxisRaw("Mouse X") * pcMouseSensitivity * Time.deltaTime;
        currentLookInput.y = Input.GetAxisRaw("Mouse Y") * pcMouseSensitivity * Time.deltaTime;

#elif UNITY_ANDROID || UNITY_IOS
        // Mobile Touch Input
        currentMovementInput = Vector2.zero; // Reset per frame
        currentLookInput = Vector2.zero;     // Reset per frame

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // If touch is over a UI element (like a jump button), ignore it for movement/look
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // If this touch was our active movement or look touch, invalidate it
                if (touch.fingerId == movementTouchId) movementTouchId = -1;
                if (touch.fingerId == lookTouchId) lookTouchId = -1;
                continue;
            }

            // Movement Area (Left half of screen)
            if (movementTouchRect.Contains(touch.position))
            {
                if (touch.phase == TouchPhase.Began && movementTouchId == -1)
                {
                    movementTouchId = touch.fingerId;
                    movementTouchStartPosition = touch.position;
                }
                else if (touch.fingerId == movementTouchId)
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        Vector2 offset = touch.position - movementTouchStartPosition;
                        // Normalize and clamp to simulate joystick input
                        // Adjust maxOffset to control sensitivity/range of virtual joystick
                        float maxOffset = Mathf.Min(Screen.width, Screen.height) * 0.15f; // e.g., 15% of smaller screen dimension
                        currentMovementInput = Vector2.ClampMagnitude(new Vector2(offset.x / maxOffset, offset.y / maxOffset), 1.0f);
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        movementTouchId = -1;
                        currentMovementInput = Vector2.zero; // Stop movement when touch is released
                    }
                }
            }
            // Look Area (Right half of screen)
            else if (lookTouchRect.Contains(touch.position))
            {
                 if (touch.phase == TouchPhase.Began && lookTouchId == -1)
                {
                    lookTouchId = touch.fingerId;
                }
                else if (touch.fingerId == lookTouchId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        currentLookInput = touch.deltaPosition * mobileLookSensitivity * Time.deltaTime;
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        lookTouchId = -1;
                    }
                }
            }
        }
#endif
    }

    void PerformGroundCheck()
    {
        Ray ray = new Ray(transform.position + groundCheckVerticalOffset, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckRayLength, groundLayer))
        {
            isGrounded = true;
            Debug.DrawRay(transform.position + groundCheckVerticalOffset, Vector3.down * groundCheckRayLength, Color.green);
        }
        else
        {
            isGrounded = false;
            Debug.DrawRay(transform.position + groundCheckVerticalOffset, Vector3.down * groundCheckRayLength, Color.red);
        }
    }

    // Public method to be called by a UI Button for jumping on mobile
    public void PublicJump()
    {
        ProcessJump();
    }

    void ProcessJump()
    {
        if (isGrounded)
        {
            rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            // isGrounded = false; // Optional: set immediately for responsiveness, raycast will confirm next frame
        }
    }

    void ApplyMovement()
    {
        Vector3 moveInputDirection = new Vector3(currentMovementInput.x, 0f, currentMovementInput.y).normalized;

        // Transform movement input from local to world space based on player's orientation
        Vector3 targetWorldVelocity = transform.TransformDirection(moveInputDirection) * moveSpeed;

        // Apply air control
        float currentControlMultiplier = isGrounded ? 1f : airControlMultiplier;

        // Smoothly change the horizontal velocity
        Vector3 horizontalVelocity = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z);
        Vector3 smoothedHorizontalVelocity = Vector3.SmoothDamp(
            horizontalVelocity,
            new Vector3(targetWorldVelocity.x, 0, targetWorldVelocity.z) * currentControlMultiplier,
            ref smoothMoveVelocity,
            accelerationTime
        );

        // If no input and grounded, try to come to a stop more quickly
        if (moveInputDirection == Vector3.zero && isGrounded)
        {
            smoothedHorizontalVelocity = Vector3.SmoothDamp(
                horizontalVelocity,
                Vector3.zero,
                ref smoothMoveVelocity,
                accelerationTime / 2f // Stop a bit faster
            );
        }

        rigidBody.linearVelocity = new Vector3(smoothedHorizontalVelocity.x, rigidBody.linearVelocity.y, smoothedHorizontalVelocity.z);
    }

    void ApplyCameraLook()
    {
        // Player body rotation (Yaw - around Y axis)
        transform.Rotate(Vector3.up * currentLookInput.x);

        // Camera pitch (around X axis)
        pitch -= currentLookInput.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) // Only draw screen-based rects if not playing, or use last known values
        {
            // These won't be accurate until Start() runs, but can give a hint
            Gizmos.color = new Color(0, 1, 0, 0.2f); // Green for movement
            // Convert screen rect to world points for gizmo (approximate for editor view)
            // This is complex to do accurately without an active camera rendering to screen.
            // For simplicity, this part is omitted. Visualizing Rects is better done with UI elements.
        }
#endif
        // Draw ground check ray
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + groundCheckVerticalOffset, transform.position + groundCheckVerticalOffset + Vector3.down * groundCheckRayLength);
    }
}