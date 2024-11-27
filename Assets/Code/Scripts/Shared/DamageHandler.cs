using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            HandleCharacterDeath();
        }
    }

    private void HandleCharacterDeath()
    {
        Destroy(gameObject);
    }
}
