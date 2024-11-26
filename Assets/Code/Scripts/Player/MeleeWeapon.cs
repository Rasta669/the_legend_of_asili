
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private PlayerInputActions _playerActions;

    private void Start()
    {
        _playerActions = FindAnyObjectByType<PlayerInputActions>();
    }
    private void OnCollisionEnter(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null && _playerActions.hasAttacked)
        {
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            enemy.Knockback(direction);
        }
    }
}
