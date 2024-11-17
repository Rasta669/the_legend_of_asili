
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            enemy.Knockback(direction);
        }
    }
}
