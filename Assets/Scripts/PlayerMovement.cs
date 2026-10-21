using UnityEngine;

// Moves the player with a Rigidbody: 
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Space to jump.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;      // units per second
    [SerializeField] private float turnSpeed = 150f;    // degrees per second
    [SerializeField] private float doubleJumpForce = 4f;
    [SerializeField] private float minJumpForce = 5f;      // strength of the jump
    [SerializeField] private float maxJumpForce = 10f;     // strength of the jump
    [SerializeField] private float maxHoldTime = 3f;      // maximum time to hold the jump button
    [SerializeField] private float runMultiplier = 2f; // Doubles the speed
    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers


    private Rigidbody rb;               // the physics body we move
    private Vector3 moveDirection;      // worked out from input in Update()
    private float turnInput;            // horizontal input for rotation
    private bool isGrounded;            // true when standing on the ground
    private bool jumpRequested = false; // set in Update(), used later
    private bool doubleJumpRequested = false; // set in TryDoubleJump()
    private bool hasDoubleJumped = false;     // one per jump
    private float currentJumpForce = 0f; // the jump force being charged up
    private float jumpHoldTime = 0f; // how long the jump button has been held down
    private bool isChargingJump; 

    public bool IsGrounded => isGrounded;

    public float SpeedMultiplier { get; set; } = 1f;
    public bool CanDoubleJump {get; set;} = false; //jump powerup

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up*groundDistance/2, -transform.up, groundDistance, groundMask);

        // Back on the ground: the double jump is available again
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }


        // 2. Read Inputs
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward


        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // Rotate the player transform in Update for responsive turning visuals
        // Calculate forward movement relative to current facing direction
        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                turnSpeed * Time.deltaTime);
        }

        // 3. Handle Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isChargingJump = true;
            jumpHoldTime = 0f;
        }

        if (isChargingJump)
        {
            if (!isGrounded)
            {
                isChargingJump = false;
            }
            else
            {
                jumpHoldTime += Time.deltaTime;
                bool realesed = Input.GetKeyUp(KeyCode.Space);
                bool timeIsUp = jumpHoldTime >= maxHoldTime;
                

                if (realesed || timeIsUp)
                {
                    float clampedHoldTime = Mathf.Clamp01(jumpHoldTime / maxHoldTime);
                    currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, clampedHoldTime);
                    jumpRequested = true;
                    isChargingJump = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
 
        if (jumpRequested)
        {
            Jump(currentJumpForce);
            currentJumpForce = 0f; // Reset jump force after jumping
            jumpRequested = false;
        }
 
        // NEW: the second jump, in mid-air
        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }
    }
 
    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f,
                                        rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }


    private void MovePlayer()
    {
        // Calculate velocity based on current forward vector
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SpeedMultiplier = 3f;
        }
        else
        {
            SpeedMultiplier = 1f;
        }

        float runFactor = Input.GetKey(KeyCode.LeftShift) ? runMultiplier : 1f;

        Vector3 targetVelocity = moveDirection * moveSpeed * SpeedMultiplier * runFactor;

        // Apply movement while preserving vertical velocity (gravity/jumping)
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

     // Tries to jump a second time in mid-air.
    // Returns true if the double jump really happened.
    public bool TryDoubleJump()
    {
        if (!CanDoubleJump) return false; // powerup requirement
        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped) return false;
 
        doubleJumpRequested = true;
        hasDoubleJumped = true;
        return true;
    }

}
