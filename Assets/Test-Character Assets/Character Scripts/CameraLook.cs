using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour
{
    [Header("References")]
    public Transform player;            // The player object
    public Transform cameraFollow;      // The GameObject indicating the camera's forward direction
    private Animator animator;          // The player's Animator component

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;    // Speed at which the player rotates
    private float lastCameraAngle = 0f; // Tracks the last camera angle
    private float currentAngleOffset = 0f; // Tracks the accumulated camera rotation angle

    [Header("Animation Settings")]
    public float angleThreshold = 45f;  // Degrees between triggering animations
    public float turnAnimationDuration = 0.5f; // Duration of the turn animation (in seconds)

    private bool isTurning = false;     // Whether a turn animation is currently playing
    private Quaternion targetRotation; // The target rotation for the player

    void Start()
    {
        // Get references
        animator = player.GetComponent<Animator>();
    }

    void Update()
    {
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (isMoving)
        {
            // When moving, make the rotation instant
            HandleInstantCameraRotation();
        }
        else if (!isTurning)
        {
            // Handle stationary camera rotation with animation when not moving
            HandleStationaryCameraRotation();
        }
        else
        {
            // Smoothly rotate to target when performing a turn animation
            SmoothlyRotateToTarget();
        }
    }

    /// <summary>
    /// Handles instant camera-based rotation while the player is moving.
    /// </summary>
    private void HandleInstantCameraRotation()
    {
        // Get the camera's horizontal forward direction (ignoring y-axis)
        Vector3 cameraForward = cameraFollow.forward;
        cameraForward.y = 0; // Ignore the vertical component
        cameraForward.Normalize();

        // Calculate the current angle difference between the camera and the player
        float cameraAngle = Quaternion.LookRotation(cameraForward).eulerAngles.y;

        // Set the target rotation instantly
        player.rotation = Quaternion.Euler(0, cameraAngle, 0);
    }

    /// <summary>
    /// Handles camera-based animation triggering while the player is stationary.
    /// </summary>
    private void HandleStationaryCameraRotation()
    {
        // Get the camera's horizontal forward direction (ignoring y-axis)
        Vector3 cameraForward = cameraFollow.forward;
        cameraForward.y = 0; // Ignore the vertical component
        cameraForward.Normalize();

        // Calculate the current angle difference between the camera and the player
        float cameraAngle = Quaternion.LookRotation(cameraForward).eulerAngles.y;
        float angleDifference = Mathf.DeltaAngle(lastCameraAngle, cameraAngle);

        // Accumulate the angle offset
        currentAngleOffset += angleDifference;

        if (Mathf.Abs(currentAngleOffset) >= angleThreshold)
        {
            // Determine whether the camera rotated left or right
            bool isRotatingRight = currentAngleOffset > 0;

            // Set the target rotation
            targetRotation = Quaternion.Euler(0, cameraAngle, 0);

            // Trigger the corresponding animation
            animator.SetTrigger(isRotatingRight ? "TurnRight" : "TurnLeft");

            // Start the rotation synchronization
            StartCoroutine(PerformTurnAnimation());

            // Reset the angle offset
            currentAngleOffset = 0f;
        }

        // Update the last camera angle
        lastCameraAngle = cameraAngle;
    }

    /// <summary>
    /// Smoothly rotates the player to the target rotation over time.
    /// </summary>
    private void SmoothlyRotateToTarget()
    {
        // Smoothly rotate the player to the target rotation
        player.rotation = Quaternion.Lerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handles the turning process and ensures the player faces the correct direction after the animation.
    /// </summary>
    private IEnumerator PerformTurnAnimation()
    {
        isTurning = true;

        // Wait for the animation to finish
        yield return new WaitForSeconds(turnAnimationDuration);

        // Ensure the player faces the target rotation at the end of the animation
        player.rotation = targetRotation;

        isTurning = false;
    }
}
