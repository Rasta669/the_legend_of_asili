
using UnityEngine;

[DefaultExecutionOrder(0)]
public class Enemies : MonoBehaviour
{
    [SerializeField] private Transform pathParent; // Parent transform containing the path points for this enemy
    [SerializeField] private float speed = 5f;
    [SerializeField] private float idleTime = 5f;

    private int pathPointIndex = 0;
    private Transform target;
    private Vector3 velocity; // Movement velocity
    private bool isGrounded;
    private CharacterController characterController;
    private Transform[] pathPoints; // Store the individual path for this enemy
    public bool hasArrived;

    void Start()
    {
        // Initialize path points from the pathParent
        if (pathParent == null)
        {
            Debug.LogError("Path parent is not assigned!");
            return;
        }

        int childCount = pathParent.childCount;
        if (childCount == 0)
        {
            Debug.LogError("Path parent has no child points!");
            return;
        }

        pathPoints = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            pathPoints[i] = pathParent.GetChild(i);
        }

        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing on the enemy!");
            return;
        }

        target = pathPoints[0];
        hasArrived = false;
    }

    void Update()
    {

        // Check if we have a valid target
        if (pathPointIndex >= pathPoints.Length || target == null)
            return;

        // Move towards the target
        Transform enemyMesh = transform.GetChild(0);
        Vector3 dir = target.position - transform.position;

        // Normalize the movement direction and scale by speed
        Vector3 movement = dir.normalized * speed;

        // Apply movement to the CharacterController
        characterController.Move(movement * Time.deltaTime);

        // Ignore Y component for rotation
        dir.y = 0;

        // Only rotate if there is a meaningful direction
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            enemyMesh.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }

        // Check if close enough to the target
        if (Vector3.Distance(target.position, transform.position) <= 0.2f && !hasArrived)
        {
            hasArrived = true; // Stop moving
            Debug.Log($"{gameObject.name} has arrived at waypoint {pathPointIndex + 1}.");
            enabled = false;
            Invoke("GetNextPoint", idleTime); // Delay to next waypoint
        }
    }

    void GetNextPoint()
    {
        Debug.Log($"{gameObject.name} getting next waypoint.");
        if (pathPointIndex < pathPoints.Length - 1)
        {
            pathPointIndex++; // Move to the next point
        }
        else
        {
            // Loop back to the starting point
            pathPointIndex = 0;
        }

        target = pathPoints[pathPointIndex];
        hasArrived = false; // Resume moving
        enabled = true;
        Debug.Log($"{gameObject.name} moving to waypoint {pathPointIndex + 1}.");
    }
    // Reset the path and resume moving
    public void RestartPath()
    {
        pathPointIndex = 0;
        target = pathPoints[0];
        hasArrived = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Reset position
    }
}
