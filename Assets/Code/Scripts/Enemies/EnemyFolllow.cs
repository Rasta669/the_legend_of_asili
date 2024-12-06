//using UnityEngine;

//public class EnemyFolllow : MonoBehaviour
//{
//    [SerializeField] Transform player;
//    [SerializeField] float speed;
//    //[SerializeField] float distance;
//    FieldOfView fov;
//    bool hasSeenPlayer;
//    [SerializeField] float OutOfFieldCountdown;
//    Enemies enemy;
//    Animator animator;
//    [SerializeField] float runBlend = 1f;
//    [SerializeField] float initialAttackBlend = 0f;
//    [SerializeField] float attackBlend = 1f;
//    public  float hitDistance = 2f;
//    public float walk = 0.5f;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        fov = GetComponent<FieldOfView>();
//        //fov = GetComponentInParent<FieldOfView>();  
//        hasSeenPlayer = false;
//        enemy = GetComponent<Enemies>();
//        animator  =  GetComponentInChildren<Animator>();
//    }

//    // Update is called once per frame
//    //void Update()
//    //{

//    //    if (player == null) {
//    //        Debug.Log("Player not found in Enemy follow script");
//    //        return;
//    //    }
//    //    if (fov.canSeePlayer)
//    //    {
//    //        animator.SetFloat("AttackParameter", initialAttackBlend);
//    //        if (Vector3.Distance(transform.position, player.position) < hitDistance)
//    //        {
//    //            animator.SetFloat("speed", initialAttackBlend);
//    //            animator.SetFloat("AttackParameter", attackBlend);
//    //            Debug.Log("Enemy attacking now");
//    //        }
//    //        else
//    //        {
//    //            hasSeenPlayer = fov.canSeePlayer;
//    //            Debug.Log("Enemy has seen player and is following");
//    //            Vector3 direction = (player.position - transform.position).normalized;
//    //            transform.Translate((direction * speed * Time.deltaTime), Space.World);
//    //            //transform.LookAt(player);
//    //            animator.SetFloat("speed", runBlend);
//    //            Debug.Log("Running");

//    //        }

//    //        if (hasSeenPlayer == false)
//    //        {
//    //            Debug.Log("Player has disappeared from enemy sight");
//    //            OutOfFieldCountdown -= Time.deltaTime;
//    //            if (OutOfFieldCountdown < 0.1)
//    //            {
//    //                enemy.restartPath();
//    //            }
//    //        }


//    //    }
//    //}

//    void Update()
//    {
//        if (player == null)
//        {
//            Debug.LogError("Player not found in Enemy follow script");
//            return;
//        }

//        if (fov.canSeePlayer)
//        {
//            // Reset attack blend if not close enough
//            animator.SetFloat("AttackParameter", initialAttackBlend);

//            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//            if (distanceToPlayer < hitDistance)
//            {
//                // Attack behavior
//                animator.SetFloat("speed", initialAttackBlend); // Stop moving
//                animator.SetFloat("AttackParameter", attackBlend); // Start attack animation
//                Debug.Log("Enemy attacking now");
//            }
//            else
//            {
//                // Follow player
//                hasSeenPlayer = true; // Remember that the player has been seen
//                Vector3 direction = (player.position - transform.position).normalized;

//                // Move towards the player
//                transform.Translate((direction * speed * Time.deltaTime), Space.World);

//                // Set movement animation
//                animator.SetFloat("speed", runBlend); // Running animation
//                Debug.Log("Enemy running towards player");
//            }
//        }
//        else if (hasSeenPlayer) // Player out of sight
//        {
//            Debug.Log("Player has disappeared from enemy sight");
//            OutOfFieldCountdown -= Time.deltaTime;

//            if (OutOfFieldCountdown <= 0)
//            {
//                enemy.restartPath(); // Call your custom restart path logic
//                hasSeenPlayer = false; // Reset sight status
//            }
//        }
//        //else
//        //{
//        //    // Patrol or idle behavior can be added here
//        //    animator.SetFloat("speed", walk); // Patrol animation
//        //    Debug.Log("Enemy patrolling");
//        //}
//    }

//}






//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
////////////////////// RIGID BODY WORKING ENEMY CONTROLLER ///////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////

// using UnityEngine;

// public class EnemyFollow : MonoBehaviour
// {
//     [SerializeField] Transform player;
//     [SerializeField] float speed = 3.0f;          // Movement speed
//     public float hitDistance = 2f;     // Distance to trigger attack
//     [SerializeField] float runBlend = 1f;        // Blend value for running
//     [SerializeField] float walkBlend = 0.5f;     // Blend value for walking
//     [SerializeField] float attackBlend = 1f;     // Blend value for attacking
//     [SerializeField] float initialAttackBlend = 0f; // Initial attack blend value
//     [SerializeField] float outOfFieldCountdown = 3f; // Time before forgetting the player
//     [SerializeField] float rotationSpeed = 5f;

//     FieldOfView fov;                             // Reference to FieldOfView script
//     Enemies enemy;                               // Reference to enemy-specific behavior
//     Animator animator;                           // Animator for controlling animations
//     bool hasSeenPlayer = false;                  // Tracks whether the player has been seen
//     float outOfFieldTimer;                       // Timer for losing sight of the player
//     public GameObject enemyMesh;                 // Shoild write code to get it auto9matically in script

//     void Start()
//     {
//         fov = GetComponent<FieldOfView>();
//         enemy = GetComponent<Enemies>();
//         animator = GetComponentInChildren<Animator>();

//         if (player == null)
//             Debug.LogError("Player reference is missing from EnemyFollow script!");
//     }

//     void Update()
//     {
//         if (player == null) return; // Exit if no player reference

//         if (fov.canSeePlayer) // Player is in the field of view
//         {
//             hasSeenPlayer = true;                  // Mark the player as seen
//             outOfFieldTimer = outOfFieldCountdown; // Reset the timer

//             float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//             if (distanceToPlayer < hitDistance) // Close enough to attack
//             {
//                 FacePlayer();
//                 AttackPlayer();
//             }
//             else // Chase the player
//             {
//                 FacePlayer();
//                 FollowPlayer();
//             }
//         }
//         else if (hasSeenPlayer) // Player out of sight but was previously seen
//         {
//             HandlePlayerOutOfSight();
//         }
//         //else // Default patrol behavior
//         //{
//         //    Patrol();
//         //}
//     }

//     void FollowPlayer()
//     {
//         Vector3 direction = (player.position - transform.position).normalized;
//         transform.Translate(direction * speed * Time.deltaTime, Space.World);

//         animator.SetFloat("speed", runBlend);         // Set running animation
//         animator.SetFloat("AttackParameter", initialAttackBlend); // Reset attack
//         Debug.Log("Enemy chasing the player");
//     }

//     void AttackPlayer()
//     {
//         animator.SetFloat("speed", initialAttackBlend); // Stop moving
//         animator.SetFloat("AttackParameter", attackBlend); // Trigger attack animation
//         Debug.Log("Enemy attacking the player");
//     }

//     void HandlePlayerOutOfSight()
//     {
//         outOfFieldTimer -= Time.deltaTime;

//         if (outOfFieldTimer <= 0) // Forget the player after countdown
//         {
//             hasSeenPlayer = false;
//             enemy.RestartPath(); // Custom restart patrol logic
//             Debug.Log("Enemy lost the player and resumed patrolling");
//         }
//         else
//         {
//             Debug.Log($"Enemy searching for the player. Time remaining: {outOfFieldTimer}");
//         }
//     }

//     void Patrol()
//     {
//         animator.SetFloat("speed", walkBlend); // Patrol animation
//         Debug.Log("Enemy patrolling");
//     }

//     // Makes the enemy face the player smoothly
//     private void FacePlayer()
//     {
//         // Calculate direction to the player while ignoring the y-axis
//         Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;

//         // Check if the direction vector is non-zero to avoid errors
//         if (direction.magnitude > 0)
//         {
//             // Calculate target rotation limited to the Y-axis
//             Quaternion targetRotation = Quaternion.LookRotation(direction);

//             // Smoothly rotate towards the player on the Y-axis only
//             enemyMesh.transform.rotation = Quaternion.Slerp(enemyMesh.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
//             transform.rotation = Quaternion.Slerp(enemyMesh.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
//         }
//     }
// }


//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
////////////////////// END OF RIGID BODY WORKING ENEMY CONTROLLER ///////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
///
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float runBlend = 1f;
    [SerializeField] static float attackBlend = 1f;
    [SerializeField] static float initialAttackBlend = 0f;
    [SerializeField] float outOfFieldCountdown = 3f;
    [SerializeField] float blendSpeed = 5f; // Speed at which animations blend
    public float hitDistance = 2f;

    private CharacterController characterController;
    private float outOfFieldTimer;
    private bool hasSeenPlayer = false;
    private Vector3 lastKnownPosition; // Last known position of the player
    private bool isSearching = false;
    private Vector3 patrolDirection; // To store the direction when the player is lost

    public bool _hasTriggeredAttack = false;

    FieldOfView fov;
    Enemies enemy;
    Animator animator;


    private void Start()
    {
        fov = GetComponent<FieldOfView>();
        enemy = GetComponent<Enemies>();
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();

        if (player == null)
            Debug.LogError("Player reference is missing!");

        if (characterController == null)
            Debug.LogError("CharacterController is missing!");

        patrolDirection = transform.forward; // Set initial patrol direction as forward
    }

    private void Update()
    {
        if (player == null) return;

        if (fov.canSeePlayer) // Player in view
        {
            enemy.enabled = false;
            hasSeenPlayer = true;
            isSearching = false; // Reset search state
            outOfFieldTimer = outOfFieldCountdown; // Reset countdown
            lastKnownPosition = player.position; // Update last known position

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= hitDistance)
            {
                FacePlayer();
                AttackPlayer();
            }
            else
            {
                FacePlayer();
                FollowPlayer();
            }
        }
        else if (hasSeenPlayer) // Player out of sight
        {
            HandlePlayerOutOfSight();
            enemy.enabled = true;
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        characterController.Move(direction * speed * Time.deltaTime);

        // Smoothly blend to the running animation
        float currentSpeed = animator.GetFloat("speed");
        animator.SetFloat("speed", Mathf.Lerp(currentSpeed, runBlend, Time.deltaTime * blendSpeed));
        animator.SetFloat("AttackParameter", Mathf.Lerp(animator.GetFloat("AttackParameter"), initialAttackBlend, Time.deltaTime * blendSpeed));

        Debug.Log("Enemy chasing the player");
    }

    void AttackPlayer()
    {
        // Smoothly blend to the attack animation
        animator.SetFloat("speed", Mathf.Lerp(animator.GetFloat("speed"), initialAttackBlend, Time.deltaTime * blendSpeed));
        animator.SetFloat("AttackParameter", Mathf.Lerp(animator.GetFloat("AttackParameter"), attackBlend, Time.deltaTime * blendSpeed));
        Debug.Log("Enemy attacking the player");
    }

    public void TriggerAttackTrue() { _hasTriggeredAttack = true; }
    public void TriggerAttackFalse() { _hasTriggeredAttack = false; }

    void HandlePlayerOutOfSight()
    {
        outOfFieldTimer -= Time.deltaTime;

        if (outOfFieldTimer > 0) // Still searching for the player
        {
            if (!isSearching)
            {
                isSearching = true; // Start searching
                MoveToLastKnownPosition(); // Move to last known position
            }
            Debug.Log($"Enemy searching for the player. Time remaining: {outOfFieldTimer}");
        }
        else // Timer expired, resume patrol
        {
            hasSeenPlayer = false;
            enemy.RestartPath(); // Custom patrol logic
            Debug.Log("Enemy lost the player and resumed patrolling");
        }

        // Smoothly rotate back to patrol direction if the player is lost
        FacePatrolDirection();
    }

    void MoveToLastKnownPosition()
    {
        Vector3 direction = (lastKnownPosition - transform.position).normalized;
        direction.y = 0;

        characterController.Move(direction * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, lastKnownPosition) < 0.5f) // Reached last known position
        {
            Debug.Log("Enemy reached the last known position of the player");
            isSearching = false; // Stop searching
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void FacePatrolDirection()
    {
        // Smoothly rotate back to the patrol direction if the player is out of sight
        Quaternion targetRotation = Quaternion.LookRotation(patrolDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}
