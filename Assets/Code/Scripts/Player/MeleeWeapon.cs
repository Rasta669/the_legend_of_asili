using System;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private Transform weaponBase;
    /* private fields *****/
    private PlayerInputActions _playerInputActions;
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

    // Store the object hit during the current attack
    private GameObject lastHitObject;

    private void Start()
    {
        _playerInputActions = FindAnyObjectByType<PlayerInputActions>();
    }

    private void Update()
    {
        if (_playerInputActions.AttackInput && isReadyToAttack)
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!isReadyToAttack) return;

        isReadyToAttack = false; // Prevent spamming attacks
        Invoke(nameof(AttackRaycast), attackDelay);
    }

    public void CycleAttack()
    {
        // Increment and loop back to 0 after a maximum of 2 attacks
        AttackCount = (AttackCount + 1) % 2;
        Debug.Log($"Cycling to next attack: {AttackCount}");
    }


    private void AttackRaycast()
    {
        if (Physics.Raycast(weaponBase.position, weaponBase.forward, out RaycastHit hitInfo, attackDistance, attackLayer))
        {
            // Ensure we hit a new object this attack
            if (hitInfo.collider.gameObject != lastHitObject)
            {
                lastHitObject = hitInfo.collider.gameObject; // Remember the hit object
                HitTarget(hitInfo);
            }
        }
    }

    private void HitTarget(RaycastHit objInfo)
    {
        Debug.Log($"Hit an object: {objInfo.collider.gameObject.name}");
        // You can apply damage or any other effect here
    }

    // Draw Gizmos to visualize ranges in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(weaponBase.position, weaponBase.forward * attackDistance);
    }
}
