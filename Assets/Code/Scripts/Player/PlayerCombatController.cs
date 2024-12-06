// using UnityEngine;

// public class PlayerCombatController : MonoBehaviour
// {

//     // ---------------------------//
//     // --- ATTACK BEHAVIOR --- //
//     // ---------------------------//

//     [Header("Attacking")]
//     [SerializeField] private float attackDistance = 3f;
//     [SerializeField] private float attackDelay = 0.4f;
//     [SerializeField] private float attackSpeed = 1f;
//     [SerializeField] private int attackDamage = 1;
//     [SerializeField] private LayerMask attackLayer;
//     [SerializeField] private Transform weaponBase;

//     // [SerializeField] private GameObject hitEffect;
//     // [SerializeField] private AudioClip swordSwing;
//     // [SerializeField] private AudioClip hitSound;


//     private bool isAttacking = false;
//     private bool isReadyToAttack = true;
//     private int attackCount;


//     // --------------- //
//     // --- METHODS --- //
//     // --------------- //
//     public void Attack()
//     {
//         if (!isReadyToAttack || isAttacking) return;

//         isReadyToAttack = false;
//         isAttacking = true;

//         // Invoke(nameof(ResetAttack), attackSpeed);
//         Invoke(nameof(AttackRaycast), attackDelay);

//         /****************************************************************************************************************************************************************/
//         if (attackCount == 0)
//         {
//             attackCount++;
//         }
//         else
//         {
//             attackCount = 0;
//         }


//     }

//     public void AttackRaycast()
//     {
//         if (Physics.Raycast(weaponBase.position, weaponBase.forward, out RaycastHit hitInfo, attackDistance, attackLayer))
//         {
//             Debug.Log($"Hit an object: {hitInfo.collider.gameObject.name}");
//             // HitTarget(hitInfo.point);
//         }
//     }

//     private void HitTarget(Vector3 pos)
//     {
//     }


//     // Draw Gizmos to visualize ranges in the Scene view
//     private void OnDrawGizmosSelected()
//     {
//         // Direction weapon raycast is facing
//         Gizmos.color = Color.blue;
//         Gizmos.DrawRay(weaponBase.position,weaponBase.forward * attackDistance);
//     }

// }
