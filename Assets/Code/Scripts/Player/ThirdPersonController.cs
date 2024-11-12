using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // Reference to the player's camera
    [SerializeField] private Camera m_PlayerCamera;

    // Input handling fields
    private PlayerInput m_playerInputAsset;  // Stores the input action asset
    private InputAction m_MoveAction;        // Movement input action

    // Movement fields
    private Rigidbody m_PlayerRB;            // Rigidbody component for physics-based movement
    [SerializeField] private float m_MoveForce = 1f;  // Force applied to move the player
    [SerializeField] private float m_JumpForce = 5f;  // Force applied for jumping
    [SerializeField] private float m_MaxSpeed = 5f;   // Maximum speed cap
    [SerializeField] private Vector3 m_ForceDirection = Vector3.zero;  // Directional force for movement

    // Attack fields
    private Animator m_Animator;                         // Animator for handling attack animations
    private readonly int _hashedAttackParam = Animator.StringToHash("Attack");  // Hashed animator parameter for attack trigger
    

    // Initialization of components and setup
    private void Awake()
    {
        m_PlayerRB = GetComponent<Rigidbody>();       // Reference to the player's Rigidbody
        m_playerInputAsset = new PlayerInput();       // Instantiate player input actions
        m_Animator = GetComponent<Animator>();        // Reference to Animator component
    }

    // Enable input actions and assign event listeners for jump and attack
    private void OnEnable()
    {
        m_playerInputAsset.Player.Jump.started += PerformJump;     // Listen for jump input
        m_playerInputAsset.Player.Attack.started += PerformAttack; // Listen for attack input
        m_MoveAction = m_playerInputAsset.Player.Move;             // Reference to movement input action
        m_playerInputAsset.Player.Enable();                        // Enable input actions
    }

    // Remove event listeners and disable input actions
    private void OnDisable()
    {
        m_playerInputAsset.Player.Jump.started -= PerformJump;     // Stop listening for jump input
        m_playerInputAsset.Player.Attack.started -= PerformAttack; // Stop listening for attack input
        m_playerInputAsset.Player.Disable();                       // Disable input actions
    }

    // Physics-based updates for movement and rotation
    private void FixedUpdate()
    {
        // Apply directional force based on camera orientation
        m_ForceDirection += m_MoveAction.ReadValue<Vector2>().x * m_MoveForce * GetCameraRight(m_PlayerCamera); // Right direction
        m_ForceDirection += m_MoveAction.ReadValue<Vector2>().y * m_MoveForce * GetCameraForward(m_PlayerCamera); // Forward direction

        m_PlayerRB.AddForce(m_ForceDirection, ForceMode.Impulse);  // Apply movement force to Rigidbody
        m_ForceDirection = Vector3.zero;                           // Reset force direction after each update

        // Apply additional gravity when player is falling
        if (m_PlayerRB.linearVelocity.y < 0f)
        {
            m_PlayerRB.linearVelocity -= Physics.gravity.y * Time.fixedDeltaTime * Vector3.down;
        }

        // Limit horizontal speed to max speed
        Vector3 horizontalVelocity = m_PlayerRB.linearVelocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > (m_MaxSpeed * m_MaxSpeed))
        {
            m_PlayerRB.linearVelocity = horizontalVelocity.normalized * m_MaxSpeed + Vector3.up * m_PlayerRB.linearVelocity.y;
        }

        // Adjust player's orientation based on movement direction
        LookAt();
    }

    // Method to orient player in the direction of movement
    private void LookAt()
    {
        Vector3 lookDir = m_PlayerRB.linearVelocity;
        lookDir.y = 0; // Restrict look direction to horizontal plane

        // Rotate if there is input and a defined look direction
        if (m_MoveAction.ReadValue<Vector2>().sqrMagnitude > 0.1f && lookDir.sqrMagnitude > 0.1f)
        {
            m_PlayerRB.rotation = Quaternion.LookRotation(lookDir, Vector3.up);  // Rotate to face movement direction
        }
        else
        {
            m_PlayerRB.angularVelocity = Vector3.zero; // Halt rotation if no input
        }
    }

    // Helper function to get forward direction based on camera orientation
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0; // Keep only horizontal component
        return forward.normalized;
    }

    // Helper function to get right direction based on camera orientation
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0; // Keep only horizontal component
        return right.normalized;
    }

    // Executes a jump if the player is grounded
    private void PerformJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            m_ForceDirection += Vector3.up * m_JumpForce;  // Add upward force for jump
        }
    }

    // Triggers attack animation
    private void PerformAttack(InputAction.CallbackContext context)
    {
        m_Animator.SetTrigger(_hashedAttackParam);  // Trigger attack animation
    }

    // Checks if player is grounded using a raycast
    private bool IsGrounded()
    {
        Ray ray = new(transform.position + Vector3.up * 0.25f, Vector3.down); // Create ray slightly above the player
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");  // Log hit object for debugging
            return true;
        }
        else
        {
            return true;
        }
    }
}
