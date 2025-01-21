//using System;
//Gravity issues
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 3.0f;
    [SerializeField] private AudioClip m_RunningAudioClip; // run audio

    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float runBlend = 1f;
    [SerializeField] float stopBlend = 0f;
    [SerializeField] static float attackBlend;
    [SerializeField] float maxAttackBlend = 1f;
    [SerializeField] static float initialAttackBlend = 0;
    [SerializeField] float outOfFieldCountdown = 3f;
    [SerializeField] float blendSpeed = 5f; // Speed at which animations blend
    public float hitDistance = 2f;

    private CharacterController characterController;
    private EnemyStats enemyStats;
    private float outOfFieldTimer;
    private bool hasSeenPlayer = false;
    private Vector3 lastKnownPosition; // Last known position of the player
    private bool isSearching = false;
    private Vector3 patrolDirection; // To store the direction when the player is lost
    float attackBlendTimer = 0.0f;

    public bool _hasTriggeredAttack = false;
    public bool _hasPlayedAttackSound = false;
    [SerializeField] private AudioClip attackSound;

    FieldOfView fov;
    Enemies enemy;
    Animator animator;


    private void Start()
    {
        fov = GetComponent<FieldOfView>();
        enemy = GetComponent<Enemies>();
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        enemyStats = GetComponent<EnemyStats>();

        if (player == null)
            Debug.LogError("Player reference is missing!");

        if (characterController == null)
            Debug.LogError("CharacterController is missing!");

        patrolDirection = transform.forward; // Set initial patrol direction as forward
    }

    private void Update()
    {
        if (enemyStats == null || enemyStats.IsDead) return; // Stop if the enemy is dead

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
                //Debug.Log("Close to the player");
                animator.SetFloat("Speed", 0);
                FacePlayer();
                //attackBlend = Random.Range(initialAttackBlend, attackBlend);
                AttackPlayer();
            }
            else
            {
                animator.SetBool("ToAttack", false);
                FacePlayer();
                FollowPlayer();
            }
        }
        else if (hasSeenPlayer) // Player out of sight
        {
            animator.SetBool("ToAttack", false);
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
        float currentSpeed = animator.GetFloat("Speed");

        //if (!SoundManager.Instance.enemyFootStepSource.isPlaying)

        //{
        //    if (currentSpeed >= 1)
        //    {
        //        // When not sprinting, reduce the pitch to make it sound like walking
        //        SoundManager.Instance.enemyFootStepSource.pitch = 1f; // Normal pitch for running
        //        SoundManager.Instance.EnemyFootStepSound(m_RunningAudioClip);
        //    }
        //    else
        //    {
        //        SoundManager.Instance.enemyFootStepSource.Stop();
        //    }
        //}
        animator.SetFloat("Speed", Mathf.Lerp(currentSpeed, runBlend, Time.deltaTime * blendSpeed));
        //animator.SetFloat("hitFactor", Mathf.Lerp(animator.GetFloat("hitFactor"), initialAttackBlend, Time.deltaTime * blendSpeed));

        Debug.Log("Enemy chasing the player");
    }

    void AttackPlayer()
    {
        // Update the attack blend value every 3 seconds
        attackBlendTimer += Time.deltaTime;
        if (attackBlendTimer >= 3f)
        {
            attackBlend = Random.Range(initialAttackBlend, maxAttackBlend);
            attackBlendTimer = 0f; // Reset the timer
        }
        if (attackBlend < 0.25)
        {
            attackBlend = 0;
        }
        else if (attackBlend >= 0.25f && attackBlend <= 0.75f)
        {
            attackBlend = 0.5f;
        }
        else
        {
            attackBlend = 1;
        }
        // Smoothly blend to the attack animation
        //animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), stopBlend, Time.deltaTime * blendSpeed));
        Debug.Log($"Enemy attacking the player with attackBlend: {attackBlend}");
        animator.SetFloat("Speed", 0);
        animator.SetBool("ToAttack", true);
        animator.SetFloat("hitFactor", Mathf.Lerp(animator.GetFloat("hitFactor"), attackBlend, Time.deltaTime * blendSpeed));
        
        //Debug.Log("Enemy attacking the player");
    }

    public void TriggerAttackTrue()
    {
        _hasTriggeredAttack = true;
        _hasPlayedAttackSound = true;
        if (_hasPlayedAttackSound && !SoundManager.Instance.attackSoundSource.isPlaying)
        {
            SoundManager.Instance.PlayAttackSound(attackSound);
        }
    }
    public void TriggerAttackFalse()
    {
        _hasTriggeredAttack = false;

        _hasPlayedAttackSound = false;
    }

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
