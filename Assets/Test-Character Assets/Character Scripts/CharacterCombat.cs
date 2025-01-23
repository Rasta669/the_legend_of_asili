using UnityEngine;
using System.Collections;

public class CharacterCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 5f; // Range within which the player can lock onto an enemy
    public float comboDelay = 0.5f; // Time window for combo input
    public Animator animator; // The player's Animator
    public float Delay = 0.1f;

    [Header("Defense Settings")]
    public string shieldUpTrigger = "ShieldUp"; // Animation trigger for raising the shield
    public string shieldDownTrigger = "ShieldDown"; // Animation trigger for lowering the shield
    public KeyCode shieldButton = KeyCode.LeftShift; // Button to hold up the shield
    public bool isShieldActive = false;

    [Header("Movement Settings")]
    public GameObject targetObject;
    private CharacterController characterController;

    [Header("Attack References")]
    private string attack1Trigger = "Attack1";
    [Header("Attack Settings")]
    public KeyCode attack1 = KeyCode.Mouse0; // Left Mouse Button
    private string attack2Trigger = "Attack2";
    public KeyCode attack2 = KeyCode.Mouse1; // Right Mouse Button
    private string attack3Trigger = "Attack3";
    public KeyCode attack3 = KeyCode.Mouse2; // Middle Mouse Button
    private string ultimateAttackTrigger = "UltimateAttack";
    public KeyCode ultimateAttack = KeyCode.Space;

    [Header("Movement Settings")]
    public float attackMoveDistance = 2f; // Distance the player moves during the attack (set in the inspector)

    private bool isAttacking = false;
    private bool isLockedOn = false;
    public int comboCount = 0;
    private float lastAttackTime = 0f;

    private Transform nearestEnemy;
    private Transform[] allEnemiesInRange;
    private int currentEnemyIndex = 0;

    void Awake()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            HandleMovement();
            HandleCombatInput();
            HandleShieldInput();
            LockOnToNearestEnemy();
        }
    }

    // Handle player movement (optional, as it may be disabled during combat)
    private void HandleMovement()
    {
        if (isLockedOn && nearestEnemy != null)
        {
            // Make sure the player faces the nearest enemy
            Vector3 directionToEnemy = nearestEnemy.position - transform.position;
            directionToEnemy.y = 0; // Keep horizontal direction only
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToEnemy), Time.deltaTime * 5f);
        }
    }

    // Handle combat input
    private void HandleCombatInput()
    {
        // Basic Attacks
        if (Input.GetKeyDown(attack1) && Time.time - lastAttackTime > comboDelay)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack(1));
        }
        else if (Input.GetKeyDown(attack2) && Time.time - lastAttackTime > comboDelay)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack(2));
        }
        else if (Input.GetKeyDown(attack3) && Time.time - lastAttackTime > comboDelay)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack(3));
        }

        // Ultimate Move
        if (Input.GetKeyDown(ultimateAttack) && Time.time - lastAttackTime > comboDelay)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformUltimateAttack());
        }

        // Switch lock-on target (if locked onto an enemy)
        if (Input.GetKeyDown(KeyCode.Tab)) // Example: Press Tab to switch between locked enemies
        {
            SwitchLockOnTarget();
        }
    }

    // Perform the attack based on the attack number
    private IEnumerator PerformAttack(int attackNumber)
    {
        isAttacking = true;

        // Trigger the attack animation
        switch (attackNumber)
        {
            case 1:
                animator.SetTrigger(attack1Trigger);
                break;
            case 2:
                animator.SetTrigger(attack2Trigger);
                break;
            case 3:
                animator.SetTrigger(attack3Trigger);
                break;
        }

        // Face the target
        if (targetObject != null)
        {
            Vector3 directionToTarget = targetObject.transform.position - transform.position;
            directionToTarget.y = 0; // Keep horizontal direction only
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }

        // Wait for the animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = false;
        comboCount = 0;
    }

    public void MoveAfterAttack()
    {
        if (characterController != null && targetObject != null)
        {
            // Calculate the direction and move toward the target object's position
            Vector3 targetPosition = targetObject.transform.position;
            Vector3 moveDirection = targetPosition - transform.position;

            // Keep movement horizontal
            moveDirection.y = 0;

            // Move the character to the target position
            characterController.Move(moveDirection);

            Debug.Log($"Moved towards {targetObject.name} at {targetPosition}");
        }
        else
        {
            Debug.LogWarning("CharacterController or TargetObject not found!");
        }
    }

    // Perform the ultimate attack
    private IEnumerator PerformUltimateAttack()
    {
        isAttacking = true;

        animator.SetTrigger(ultimateAttackTrigger);

        // Wait for the ultimate attack animation to complete
        yield return new WaitForSeconds(3f); // Adjust this duration based on animation length

        isAttacking = false;
    }

    // Handle shield input
    private void HandleShieldInput()
    {
        if (Input.GetKeyDown(shieldButton) && !isShieldActive)
        {
            //Activate Shield
                isShieldActive = true;
                animator.SetBool("shieldUpTrigger", true);
                animator.SetBool("shieldDownTrigger", false);
        }
        else if (Input.GetKeyDown(shieldButton) && isShieldActive)
        {
            // Deactivate shield
                isShieldActive = false;
                animator.SetBool("shieldDownTrigger", true);
                animator.SetBool("shieldUpTrigger", false);
        }
    }

    // Lock on to the nearest enemy
    private void LockOnToNearestEnemy()
    {
        // Search for enemies within range
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange);
        allEnemiesInRange = new Transform[enemiesInRange.Length];

        int enemyCount = 0;
        foreach (var enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                allEnemiesInRange[enemyCount] = enemy.transform;
                enemyCount++;
            }
        }

        // If there are enemies in range, lock onto the nearest one
        if (enemyCount > 0)
        {
            if (!isLockedOn)
            {
                nearestEnemy = allEnemiesInRange[0];
                currentEnemyIndex = 0;
                isLockedOn = true;
            }

            // Determine the closest enemy
            float closestDistance = Mathf.Infinity;
            foreach (var enemy in allEnemiesInRange)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);
                if (distanceToEnemy < closestDistance)
                {
                    nearestEnemy = enemy;
                    closestDistance = distanceToEnemy;
                }
            }

            // Make sure the player faces the locked-on enemy
            Vector3 directionToEnemy = nearestEnemy.position - transform.position;
            directionToEnemy.y = 0; // Keep horizontal direction only
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToEnemy), Time.deltaTime * 5f);
        }
        else
        {
            isLockedOn = false;
            nearestEnemy = null;
        }
    }

    // Switch lock-on target
    private void SwitchLockOnTarget()
    {
        if (allEnemiesInRange.Length == 0)
            return;

        // Switch the lock-on to the next enemy in the list
        currentEnemyIndex = (currentEnemyIndex + 1) % allEnemiesInRange.Length;
        nearestEnemy = allEnemiesInRange[currentEnemyIndex];

        // Make sure the player faces the new target
        Vector3 directionToEnemy = nearestEnemy.position - transform.position;
        directionToEnemy.y = 0; // Keep horizontal direction only
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToEnemy), Time.deltaTime * 5f);
    }

    // Toggle lock-on state manually (if needed)
    public void ToggleLockOn()
    {
        isLockedOn = !isLockedOn;
    }
}
