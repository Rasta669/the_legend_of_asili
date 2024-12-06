///
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 3.0f;
    [SerializeField] private AudioClip m_RunningAudioClip; // run audio

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

        if (!SoundManager.Instance.enemyFootStepSource.isPlaying)

        {
            if (currentSpeed >= 1)
            {
                // When not sprinting, reduce the pitch to make it sound like walking
                SoundManager.Instance.enemyFootStepSource.pitch = 1f; // Normal pitch for running
                SoundManager.Instance.EnemyFootStepSound(m_RunningAudioClip);
            }
            else
            {
                SoundManager.Instance.enemyFootStepSource.Stop();
            }
        }
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
