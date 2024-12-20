using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterGravityController : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float gravity = -9.81f; // Gravity force
    public float groundCheckDistance = 0.2f; // Distance to check for ground
    public LayerMask groundLayer; // Layer mask for ground detection
    public float jumpHeight = 2.0f; // Jump height

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleGravityAndGrounding();
    }

    private void HandleGravityAndGrounding()
    {
        // Ground check using a sphere cast
        Vector3 groundCheckPosition = transform.position + Vector3.down * groundCheckDistance;
        isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to keep the character grounded
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply the calculated velocity to the CharacterController
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        // Visualize the ground check in the editor
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckDistance);
    }
}
