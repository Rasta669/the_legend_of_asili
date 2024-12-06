using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private EnemyFollow _enemyFollow;
    private BoxCollider _weaponCollider; // Collider attached to the weapon
    private bool hasDamaged = false; // Flag to prevent multiple damage applications


    private void Start()
    {
        _weaponCollider = GetComponent<BoxCollider>();

        if (_weaponCollider == null)
        {
            Debug.LogError("No Attack Collider attached to the melee weapon.");
        }
        else
        {
            Debug.Log("Attack Collider found and ready.");
            _weaponCollider.enabled = false; // Ensure the collider is disabled initially
        }
    }

    private void Update()
    {
        // Trigger the collider when attack starts
        if (_enemyFollow._hasTriggeredAttack && !_weaponCollider.enabled)
        {
            EnableWeaponCollider();
        }
        // Disable the collider when attack is finished
        else if (!_enemyFollow._hasTriggeredAttack && _weaponCollider.enabled)
        {
            DisableWeaponCollider();
        }
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
        _enemyFollow.TriggerAttackFalse();
        // Check if it's the enemy and the damage hasn't been applied yet
        if (other.CompareTag("Player") && !hasDamaged)
        {
            var playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
                hasDamaged = true; // Set the flag to prevent multiple damage applications
                DisableWeaponCollider(); // Immediately disable the collider after damage
            }
            else
            {
                Debug.LogWarning("PlayerStats component not found on the hit object.");
            }
        }
    }
}
