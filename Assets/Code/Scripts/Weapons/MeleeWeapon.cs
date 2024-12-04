// using System;
// using UnityEngine;

// public class MeleeWeapon : MonoBehaviour
// {
//     [Header("Attacking")]
//     [SerializeField] private int damageAmount = 10;
//     [SerializeField] private float attackDistance = 3f;
//     [SerializeField] private float attackDelay = 0.4f;
//     [SerializeField] private LayerMask attackLayer;
//     [SerializeField] private Transform weaponBase;
//     /* private fields *****/
//     private PlayerInputActions _playerInputActions;
//     public bool isReadyToAttack = true;
//     private int attackCount;
//     public int AttackCount
//     {
//         get => attackCount;
//         set
//         {
//             attackCount = value;
//             Debug.Log($"Attack Count updated to: {attackCount}");
//         }
//     }

//     // Store the object hit during the current attack
//     private GameObject lastHitObject;

//     private void Start()
//     {
//         _playerInputActions = FindAnyObjectByType<PlayerInputActions>();
//     }

//     private void Update()
//     {
//         if (_playerInputActions.AttackInput && isReadyToAttack)
//         {
//             Attack();
//         }
//     }

//     public void Attack()
//     {
//         if (!isReadyToAttack) return;

//         isReadyToAttack = false; // Prevent spamming attacks
//         Invoke(nameof(AttackRaycast), attackDelay);
//     }

//     public void CycleAttack()
//     {
//         // Increment and loop back to 0 after a maximum of 2 attacks
//         AttackCount = (AttackCount + 1) % 2;
//         Debug.Log($"Cycling to next attack: {AttackCount}");
//         Debug.Log($"Resetting ready to attack.");
//         ResetAttack();
//     }


//     private void AttackRaycast()
//     {
//         if (Physics.Raycast(weaponBase.position, weaponBase.forward, out RaycastHit hitInfo, attackDistance, attackLayer))
//         {

//             // You can apply damage or any other effect here
//             Debug.Log($"Hit an object: {hitInfo.collider.gameObject.name}");
//             // Ensure we hit a new object this attack
//             if (hitInfo.collider.gameObject != lastHitObject)
//             {
//                 lastHitObject = hitInfo.collider.gameObject; // Remember the hit object
//                 HitTarget(hitInfo);
//             }
//         }
//     }
//     private void ResetAttack()
//     {
//         isReadyToAttack = true;
//     }

//     private void HitTarget(RaycastHit objInfo)
//     {
//         if (objInfo.collider.CompareTag("Enemy"))
//         {
//             objInfo.collider.GetComponent<EnemyStats>().TakeDamage(damageAmount);
//         }
//     }

//     // Draw Gizmos to visualize ranges in the Scene view
//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.blue;
//         Gizmos.DrawRay(weaponBase.position, weaponBase.forward * attackDistance);
//     }
// }


using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private LayerMask attackLayer; // Specify layers for valid targets
    private PlayerInputActions _playerInputActions;
    private BoxCollider _weaponCollider; // Collider attached to the weapon
    private bool hasAttacked = false; // Track attack state
    public bool isReadyToAttack = true;
    private int attackCount;
    public int AttackCount
    {
        get => attackCount;
        set
        {
            attackCount = value;
            Debug.Log($"Attack Count updated to: {attackCount}");
        }
    }
    private void Start()
    {
        _playerInputActions = FindAnyObjectByType<PlayerInputActions>();
        _weaponCollider = GetComponent<BoxCollider>();

        if (_weaponCollider == null)
        {
            Debug.LogError("No Collider attached to the melee weapon.");
            return;
        }

        _weaponCollider.enabled = false; // Ensure the collider is disabled initially
    }

    private void Update()
    {
        // Trigger collider on attack start
        if (_playerInputActions.AttackInput && !hasAttacked)
        {
            EnableWeaponCollider();
        }
    }


    public void CycleAttack()
    {
        // Increment and loop back to 0 after a maximum of 2 attacks
        AttackCount = (AttackCount + 1) % 2;
        Debug.Log($"Cycling to next attack: {AttackCount}");
        Debug.Log($"Resetting ready to attack.");
        isReadyToAttack = true; // reset attack
    }

    // Enable the weapon's collider for a short duration during an attack
    private void EnableWeaponCollider()
    {
        hasAttacked = true;
        _weaponCollider.enabled = true;

        // Schedule the collider to be turned off
        Invoke(nameof(DisableWeaponCollider), 0.5f); // Adjust duration based on your animation
    }

    // Disable the weapon's collider
    public void DisableWeaponCollider()
    {
        _weaponCollider.enabled = false;
        hasAttacked = false;
    }

    // Trigger collision detection for damage application
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Enemy"))
    //         {
    // Debug.Log($"Hit enemy: {other.gameObject.name}");
    //             var enemyStats = other.GetComponent<EnemyStats>();
    //             if (enemyStats != null)
    //             {
    //                 enemyStats.TakeDamage(damageAmount);
    //             }
    //             else
    //             {
    //                 Debug.LogWarning("EnemyStats component not found on the hit object.");
    //             }
    //         }

    // }

    private void OnCollisionEnter(Collision other)
    {
        if (((1 << other.gameObject.layer) & attackLayer) != 0) // Check if object is in attackLayer
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                var enemyStats = other.gameObject.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(damageAmount);
                    Debug.Log($"Hit enemy: {other.gameObject.name}");
                }
                else
                {
                    Debug.LogWarning("EnemyStats component not found on the hit object.");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the ground check in the editor
        Gizmos.color = Color.yellow;
        if (_weaponCollider != null)
        {
            Gizmos.DrawWireCube(_weaponCollider.transform.position, _weaponCollider.size);
        }
    }
}
