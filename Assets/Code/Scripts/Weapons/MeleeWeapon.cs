using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private int damageAmount = 10;
    private PlayerInputActions _playerInputActions;
    private MeshCollider _weaponCollider; // Collider attached to the weapon
    private bool hasDamaged = false; // Flag to prevent multiple damage applications

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
        _weaponCollider = GetComponent<MeshCollider>();

        if (_weaponCollider == null)
        {
            Debug.LogError("No Collider attached to the melee weapon.");
        }
        else
        {
            Debug.Log("Collider found and ready.");
            _weaponCollider.enabled = false; // Ensure the collider is disabled initially
        }
    }

    private void Update()
    {
        // Trigger the collider when attack starts
        if (_playerInputActions.hasAttacked && !_weaponCollider.enabled)
        {
            EnableWeaponCollider();
        }
        // Disable the collider when attack is finished
        else if (!_playerInputActions.hasAttacked && _weaponCollider.enabled)
        {
            DisableWeaponCollider();
        }
    }

    public void CycleAttack()
    {
        // Increment and loop back to 0 after a maximum of 2 attacks
        AttackCount = (AttackCount + 1) % 2;
        Debug.Log($"Cycling to next attack: {AttackCount}");
        Debug.Log($"Resetting ready to attack.");
    }

    // Enable the weapon's collider for a short duration during an attack
    private void EnableWeaponCollider()
    {
        _weaponCollider.enabled = true;
        hasDamaged = false; // Reset the damage flag when enabling the collider
    }

    // Disable the weapon's collider
    public void DisableWeaponCollider()
    {
        _weaponCollider.enabled = false;
    }

    // Trigger collision detection for damage application
    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the enemy and the damage hasn't been applied yet
        if (other.CompareTag("Enemy") && !hasDamaged)
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damageAmount);
                hasDamaged = true; // Set the flag to prevent multiple damage applications
                DisableWeaponCollider(); // Immediately disable the collider after damage
                _playerInputActions.SetHasAttackedFalse();
            }
            else
            {
                Debug.LogWarning("EnemyStats component not found on the hit object.");
            }
        }
    }
}
