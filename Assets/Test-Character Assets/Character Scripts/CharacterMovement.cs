using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6.0f; // Movement speed
    public float jumpHeight = 1.5f; // Height of the jump
    public float gravity = -9.81f; // Gravity value

    [Header("References")]
    public Animator animator;
    private CharacterController controller;

    // Movement and jump state
    private Vector3 velocity;
    private bool isGrounded = true;
    private bool isCrouching = false;
    private bool isMoving = false;

    [Header("Ground Check")]
    public Transform groundCheck; // Empty object to check if grounded
    public float groundDistance = 0.4f; // Radius for ground check
    public LayerMask groundMask; // Layers considered as ground

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleGroundCheck();
        HandleInput();
        HandleAnimation();
    }

    private void HandleGroundCheck()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensure the player sticks to the ground
        }
    }

    private void HandleInput()
    {
        // Movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // Determine if the player is moving
        isMoving = move.magnitude > 0.1f;

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump(moveX, moveZ);
        }

        // Crouch input
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            animator.SetBool("IsCrouching", isCrouching);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the character (gravity)
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleAnimation()
    {
        // Set grounded and crouching states
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsCrouching", isCrouching);

        // Set movement state
        animator.SetBool("IsMoving", isMoving);

        // Set movement speed for blending
        float movementSpeed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        animator.SetFloat("Speed", movementSpeed);
    }

    private void Jump(float moveX, float moveZ)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Trigger the correct jump animation
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            animator.SetTrigger("MovingJump"); // Jump while moving
        }
        else
        {
            animator.SetTrigger("IdleJump"); // Jump while standing still
        }
    }
}
