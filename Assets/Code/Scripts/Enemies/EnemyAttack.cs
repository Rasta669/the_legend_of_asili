using UnityEngine;
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollideer;

    private EnemyFollow _enemyFollow;
    private bool _hasDamaged = false;

    private void Start()
    {
        _enemyFollow = GetComponent<EnemyFollow>();
        if (boxCollideer == null)
        {
            Debug.LogError("No Attack Collider attached to the melee weapon.");
        }
        else
        {
            Debug.Log("Attack Collider found and ready.");
            boxCollideer.enabled = false; // Ensure the collider is disabled initially
        }
    }
    private void Update()
    {

        if (_enemyFollow._hasTriggeredAttack && !boxCollideer.enabled)
        {
            Debug.Log($"{gameObject.name} is attacking...");
            EnableCollider();
        }
        else if (!_enemyFollow._hasTriggeredAttack && boxCollideer.enabled)
        {
            DisableCollider();
        }
    }

    private void EnableCollider()
    {
        boxCollideer.enabled = true;
        _hasDamaged = true;
    }
    private void DisableCollider()
    {
        boxCollideer.enabled = false;
        _hasDamaged = false;
    }
}
