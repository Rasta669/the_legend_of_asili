using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private LayerMask attackLayer; // Specify layers for valid targets (e.g., Player)
    private BoxCollider _attackCollider; // Collider for the enemy's attack hitbox

    private void Start()
    {
        _attackCollider = GetComponent<BoxCollider>();
        if (_attackCollider == null)
        {
            Debug.LogError("No Collider attached to the enemy attack object.");
            return;
        }
        // _attackCollider.gameObject.SetActive(false);
    }

    // Animation event: Enable enemy's attack collider
    public void EnableAttackCollider()
    {
        _attackCollider.gameObject.SetActive(true);
        Debug.Log("Enemy attack collider enabled.");
    }

    // Animation event: Disable enemy's attack collider
    public void DisableAttackCollider()
    {
        _attackCollider.gameObject.SetActive(false);
        Debug.Log("Enemy attack collider disabled.");
    }

    // Detect collisions and apply damage during attack
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object belongs to the attackLayer (e.g., the player layer)
        if (((1 << other.gameObject.layer) & attackLayer) != 0)
        {
            if (other.CompareTag("Player"))
            {
                // If the target has PlayerStats, deal damage
                var playerStats = other.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damageAmount);
                    Debug.Log($"Enemy dealt {damageAmount} damage to {other.gameObject.name}.");
                }
                else
                {
                    Debug.LogWarning("PlayerStats component not found on the hit object.");
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        // Visualize the ground check in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_attackCollider.transform.position, _attackCollider.size);
    }
}
