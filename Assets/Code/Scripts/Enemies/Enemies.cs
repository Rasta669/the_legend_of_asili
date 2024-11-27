using UnityEngine;

[DefaultExecutionOrder(0)]
public class Enemies : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private int pathPointIndex = 0;
    private Transform target;
    [SerializeField] private float idleTime = 5f;
    public static bool hasArrived;

    void Start()
    {
        // Ensure path is initialized
        if (EnemyPath.path == null || EnemyPath.path.Length == 0)
        {
            Debug.LogError("EnemyPath.path is not initialized or empty!");
            return;
        }
        foreach (var point in EnemyPath.path)
        {
            if (point == null)
            {
                Debug.LogError("One of the points in EnemyPath.path is null!");
                return;
            }
        }
        target = EnemyPath.path[0];
        hasArrived = false;
    }

    void Update()
    {
        // Check if we have a valid target
        if (pathPointIndex >= EnemyPath.path.Length || target == null)
            return;

        // Move towards the target
        Transform enemyMesh = transform.GetChild(0);
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Ignore Y component to ensure it only rotates on the Y-axis
        dir.y = 0;

        // Only rotate if there is a meaningful direction
        if (dir.sqrMagnitude > 0.01f)
        {
            // Create a rotation that faces the target, but only rotates around the Y-axis
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            // Apply only the Y component of the rotation, preserving the original X and Z rotation
            enemyMesh.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }


        // Check if close enough to the target
        if (Vector3.Distance(target.position, transform.position) <= 0.2f && !hasArrived)
        {
            hasArrived = true; // Stop moving
            Debug.Log("Enemy has arrived at the waypoint.");
            enabled = false;
            Invoke("GetNextPoint", idleTime); // Delay to next waypoint
        }
    }

    
    void GetNextPoint()
    {
        Debug.Log("Getting next waypoint.");
        if (pathPointIndex < EnemyPath.path.Length - 1)
        {
            pathPointIndex++; // Move to the next point
            target = EnemyPath.path[pathPointIndex];
            hasArrived = false; // Resume moving
            enabled = true;    
            Debug.Log("Moving to the next waypoint.");
        }
        else
        {
            Debug.Log("No more waypoints.");
        }
    }

    //should be a static method
    public void restartPath()
    {
        pathPointIndex = 0;
        target = EnemyPath.path[0];
        hasArrived = false;

    }
}
