using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // ATTRIBUTES
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _testAmount;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private TextMeshProUGUI _healthText;


    private float _currentHealth;


    // STARTUP
    private void Start()
    {
        _currentHealth = _maxHealth;
        if (_healthBar != null)
        {
            _healthBar.SetSliderMax(_maxHealth);
        }
    }

    // METHODS
    public void Update()
    {
        DisplayHealth();
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            HandleCharacterDeath();
        }
    }

    private void DisplayHealth()
    {
        _healthText.text = _currentHealth + " / " + _maxHealth;
    }
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        _healthBar.SetSlider(_currentHealth);
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        _healthBar.SetSlider(_currentHealth);
    }

    private void HandleCharacterDeath()
    {
        // play animation
        // play death sound
        // activate death screen
        Debug.Log("You're dead...");
    }
}
