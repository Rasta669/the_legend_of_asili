//using UnityEngine;

//[DefaultExecutionOrder(0)]
//public class Enemies : MonoBehaviour
//{
//    [SerializeField] private float speed = 5f;
//    private int pathPointIndex = 0;
//    private Transform target;
//    [SerializeField] private float idleTime = 5f;
//    public static bool hasArrived;

//    void Start()
//    {
//        // Ensure path is initialized
//        if (EnemyPath.path == null || EnemyPath.path.Length == 0)
//        {
//            Debug.LogError("EnemyPath.path is not initialized or empty!");
//            return;
//        }
//        foreach (var point in EnemyPath.path)
//        {
//            if (point == null)
//            {
//                Debug.LogError("One of the points in EnemyPath.path is null!");
//                return;
//            }
//        }
//        target = EnemyPath.path[0];
//        hasArrived = false;
//    }

//    //void Update()
//    //{
//    //    // Check if we have a valid target
//    //    if (pathPointIndex >= EnemyPath.path.Length || target == null)
//    //        return;

//    //    // Move towards the target
//    //    Transform enemyMesh = transform.GetChild(0);
//    //    Vector3 dir = target.position - transform.position;
//    //    transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

//    //    // Ignore Y component to ensure it only rotates on the Y-axis
//    //    dir.y = 0;

//    //    // Only rotate if there is a meaningful direction
//    //    if (dir.sqrMagnitude > 0.01f)
//    //    {
//    //        // Create a rotation that faces the target, but only rotates around the Y-axis
//    //        Quaternion targetRotation = Quaternion.LookRotation(dir);
//    //        // Apply only the Y component of the rotation, preserving the original X and Z rotation
//    //        enemyMesh.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//    //        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//    //    }


//    //    // Check if close enough to the target
//    //    if (Vector3.Distance(target.position, transform.position) <= 0.2f && !hasArrived)
//    //    {
//    //        hasArrived = true; // Stop moving
//    //        Debug.Log("Enemy has arrived at the waypoint.");
//    //        enabled = false;
//    //        Invoke("GetNextPoint", idleTime); // Delay to next waypoint
//    //    }
//    //}


//    void Update()
//    {
//        // Check if we have a valid target
//        if (pathPointIndex >= EnemyPath.path.Length || target == null)
//            return;

//        // Move towards the target
//        Transform enemyMesh = transform.GetChild(0);
//        Vector3 dir = target.position - transform.position;
//        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

//        // Ignore Y component to ensure it only rotates on the Y-axis
//        dir.y = 0;

//        // Only rotate if there is a meaningful direction
//        if (dir.sqrMagnitude > 0.01f)
//        {
//            // Create a rotation that faces the target, but only rotates around the Y-axis
//            Quaternion targetRotation = Quaternion.LookRotation(dir);
//            // Apply only the Y component of the rotation, preserving the original X and Z rotation
//            enemyMesh.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//        }


//        // Check if close enough to the target
//        if (Vector3.Distance(target.position, transform.position) <= 0.2f && !hasArrived)
//        {
//            hasArrived = true; // Stop moving
//            Debug.Log("Enemy has arrived at the waypoint.");
//            enabled = false;
//            Invoke("GetNextPoint", idleTime); // Delay to next waypoint
//        }
//    }

//    void GetNextPoint()
//    {
//        Debug.Log("Getting next waypoint.");
//        if (pathPointIndex < EnemyPath.path.Length - 1)
//        {
//            pathPointIndex++; // Move to the next point
//            target = EnemyPath.path[pathPointIndex];
//            hasArrived = false; // Resume moving
//            enabled = true;    
//            Debug.Log("Moving to the next waypoint.");
//        }
//        else 
//        {
//            Debug.Log("No more waypoints.");
//        }
//    }

//    //should be a static method
//    public void restartPath()
//    {
//        pathPointIndex = 0;
//        target = EnemyPath.path[0];
//        hasArrived = false;

//    }
//}



//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
////////////////////// RIGID BODY WORKING ENEMY CONTROLLER ///////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
// using UnityEngine;

// [DefaultExecutionOrder(0)]
// public class Enemies : MonoBehaviour
// {
//     [SerializeField] private float speed = 5f;
//     private int pathPointIndex = 0;
//     private Transform target;
//     [SerializeField] private float idleTime = 5f;
//     public static bool hasArrived;
//     private float initialY; // Store the initial Y position to prevent floating

//     void Start()
//     {
//         // Ensure path is initialized
//         if (EnemyPath.path == null || EnemyPath.path.Length == 0)
//         {
//             Debug.LogError("EnemyPath.path is not initialized or empty!");
//             return;
//         }
//         foreach (var point in EnemyPath.path)
//         {
//             if (point == null)
//             {
//                 Debug.LogError("One of the points in EnemyPath.path is null!");
//                 return;
//             }
//         }

//         initialY = transform.position.y; // Store the grounded Y position
//         target = EnemyPath.path[0];
//         hasArrived = false;
//     }

//     void Update()
//     {
//         // Check if we have a valid target
//         if (pathPointIndex >= EnemyPath.path.Length || target == null)
//             return;

//         // Move towards the target
//         Transform enemyMesh = transform.GetChild(0);
//         Vector3 dir = target.position - transform.position;

//         // Preserve the initial Y position
//         float fixedY = initialY;

//         // Translate while maintaining Y position
//         transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

//         // Reset Y position to prevent floating
//         Vector3 newPosition = transform.position;
//         newPosition.y = fixedY;
//         transform.position = newPosition;

//         // Ignore Y component for rotation
//         dir.y = 0;

//         // Only rotate if there is a meaningful direction
//         if (dir.sqrMagnitude > 0.01f)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(dir);
//             enemyMesh.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//             transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//         }

//         // Check if close enough to the target
//         if (Vector3.Distance(target.position, transform.position) <= 0.2f && !hasArrived)
//         {
//             hasArrived = true; // Stop moving
//             Debug.Log("Enemy has arrived at the waypoint.");
//             enabled = false;
//             Invoke("GetNextPoint", idleTime); // Delay to next waypoint
//         }
//     }

//     void GetNextPoint()
//     {
//         Debug.Log("Getting next waypoint.");
//         if (pathPointIndex < EnemyPath.path.Length - 1)
//         {
//             pathPointIndex++; // Move to the next point
//         }
//         else
//         {
//             // Loop back to the starting point
//             pathPointIndex = 0;
//         }

//         target = EnemyPath.path[pathPointIndex];
//         hasArrived = false; // Resume moving
//         enabled = true;
//         Debug.Log($"Moving to waypoint {pathPointIndex + 1}.");
//     }

//     // Reset the path and resume moving
//     public void RestartPath()
//     {
//         pathPointIndex = 0;
//         target = EnemyPath.path[0];
//         hasArrived = false;
//         transform.position = new Vector3(transform.position.x, initialY, transform.position.z); // Reset Y position
//     }
// }
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
///
//using UnityEngine;

//[DefaultExecutionOrder(0)]
//public class Enemies : MonoBehaviour
//{
//    [SerializeField] private float speed = 5f;
//    [SerializeField] private float gravity = -9.81f; // Gravity force
//    [SerializeField] private float idleTime = 5f;
//    [SerializeField] private float groundCheckDistance = 0.2f; // Distance to check for ground
//    [SerializeField] private LayerMask groundLayer; // Layer for ground detection

//    private int pathPointIndex = 0;
//    private Transform target;
//    private Vector3 velocity; // Movement velocity
//    private bool isGrounded;
//    private CharacterController characterController;

//    public static bool hasArrived;


//    void Start()
//    {
//        // Ensure path is initialized
//        if (EnemyPath.path == null || EnemyPath.path.Length == 0)
//        {
//            Debug.LogError("EnemyPath.path is not initialized or empty!");
//            return;
//        }
//        foreach (var point in EnemyPath.path)
//        {
//            if (point == null)
//            {
//                Debug.LogError("One of the points in EnemyPath.path is null!");
//                return;
//            }
//        }

//        characterController = GetComponent<CharacterController>();
//        if (characterController == null)
//        {
//            Debug.LogError("CharacterController component is missing on the enemy!");
//            return;
//        }

//        target = EnemyPath.path[0];
//        hasArrived = false;
//    }

//    void Update()
//    {
//        // Ground check using a raycast
//        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckDistance, groundLayer);

//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f; // Reset vertical velocity to stay grounded
//        }

//        // Apply gravity
//        velocity.y += gravity * Time.deltaTime;

//        // Check if we have a valid target
//        if (pathPointIndex >= EnemyPath.path.Length || target == null)
//            return;

//        // Move towards the target
//        Transform enemyMesh = transform.GetChild(0);
//        Vector3 dir = target.position - transform.position;

//        // Normalize the movement direction and scale by speed
//        Vector3 movement = dir.normalized * speed;

//        // Add vertical velocity
//        movement.y = velocity.y;

//        // Apply movement to the CharacterController
//        characterController.Move(movement * Time.deltaTime);

//        // Ignore Y component for rotation
//        dir.y = 0;

//        // Only rotate if there is a meaningful direction
//        if (dir.sqrMagnitude > 0.01f)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(dir);
//            enemyMesh.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//        }

//        // Check if close enough to the target
//        if (Vector3.Distance(target.position, transform.position) <= 0.2f && !hasArrived)
//        {
//            hasArrived = true; // Stop moving
//            Debug.Log("Enemy has arrived at the waypoint.");
//            enabled = false;
//            Invoke("GetNextPoint", idleTime); // Delay to next waypoint
//        }
//    }

//    void GetNextPoint()
//    {
//        Debug.Log("Getting next waypoint.");
//        if (pathPointIndex < EnemyPath.path.Length - 1)
//        {
//            pathPointIndex++; // Move to the next point
//        }
//        else
//        {
//            // Loop back to the starting point
//            pathPointIndex = 0;
//        }

//        target = EnemyPath.path[pathPointIndex];
//        hasArrived = false; // Resume moving
//        enabled = true;
//        Debug.Log($"Moving to waypoint {pathPointIndex + 1}.");
//    }

//    // Visualize the ground check in the editor
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = isGrounded ? Color.green : Color.red;
//        Gizmos.DrawSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckDistance);
//    }

//    // Reset the path and resume moving
//    public void RestartPath()
//    {
//        pathPointIndex = 0;
//        target = EnemyPath.path[0];
//        hasArrived = false;
//        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Reset position
//    }
//}



//using UnityEngine;

//[DefaultExecutionOrder(0)]
//public class Enemies : MonoBehaviour
//{
//    [SerializeField] private Transform pathParent; // Parent transform containing the path points
//    [SerializeField] private float speed = 5f;
//    [SerializeField] private float gravity = -9.81f; // Gravity force
//    [SerializeField] private float idleTime = 5f;
//    [SerializeField] private float groundCheckDistance = 0.2f; // Distance to check for ground
//    [SerializeField] private LayerMask groundLayer; // Layer for ground detection

//    private int pathPointIndex = 0;
//    private Transform target;
//    private Vector3 velocity; // Movement velocity
//    private bool isGrounded;
//    private CharacterController characterController;
//    //EnemyPath EnemyPath;
//    public static bool hasArrived;

//    void Start()
//    {
//        //EnemyPath = pathParent.GetComponent<EnemyPath>();
//        // Ensure path is initialized
//        if (EnemyPath.path == null || EnemyPath.path.Length == 0)
//        {
//            Debug.LogError("EnemyPath.path is not initialized or empty!");
//            return;
//        }
//        foreach (var point in EnemyPath.path)
//        {
//            if (point == null)
//            {
//                Debug.LogError("One of the points in EnemyPath.path is null!");
//                return;
//            }
//        }

//        characterController = GetComponent<CharacterController>();
//        if (characterController == null)
//        {
//            Debug.LogError("CharacterController component is missing on the enemy!");
//            return;
//        }

//        target = EnemyPath.path[0];
//        hasArrived = false;
//    }

//    void Update()
//    {
//        // Ground check using a raycast
//        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckDistance, groundLayer);

//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f; // Reset vertical velocity to stay grounded
//        }

//        // Apply gravity
//        velocity.y += gravity * Time.deltaTime;

//        // Check if we have a valid target
//        if (pathPointIndex >= EnemyPath.path.Length || target == null)
//            return;

//        // Move towards the target
//        Transform enemyMesh = transform.GetChild(0);
//        Vector3 dir = target.position - transform.position;

//        // Normalize the movement direction and scale by speed
//        Vector3 movement = dir.normalized * speed;

//        // Add vertical velocity
//        movement.y = velocity.y;

//        // Apply movement to the CharacterController
//        characterController.Move(movement * Time.deltaTime);

//        // Ignore Y component for rotation
//        dir.y = 0;

//        // Only rotate if there is a meaningful direction
//        if (dir.sqrMagnitude > 0.01f)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(dir);
//            enemyMesh.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
//        }

//        // Check if close enough to the target
//        if (Vector3.Distance(target.position, transform.position) <= 0.2f && !hasArrived)
//        {
//            hasArrived = true; // Stop moving
//            Debug.Log("Enemy has arrived at the waypoint.");
//            enabled = false;
//            Invoke("GetNextPoint", idleTime); // Delay to next waypoint
//        }
//    }

//    void GetNextPoint()
//    {
//        Debug.Log("Getting next waypoint.");
//        if (pathPointIndex < EnemyPath.path.Length - 1)
//        {
//            pathPointIndex++; // Move to the next point
//        }
//        else
//        {
//            // Loop back to the starting point
//            pathPointIndex = 0;
//        }

//        target = EnemyPath.path[pathPointIndex];
//        hasArrived = false; // Resume moving
//        enabled = true;
//        Debug.Log($"Moving to waypoint {pathPointIndex + 1}.");
//    }

//    // Visualize the ground check in the editor
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = isGrounded ? Color.green : Color.red;
//        Gizmos.DrawSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckDistance);
//    }

//    // Reset the path and resume moving
//    public void RestartPath()
//    {
//        pathPointIndex = 0;
//        target = EnemyPath.path[0];
//        hasArrived = false;
//        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Reset position
//    }
//}


//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
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
