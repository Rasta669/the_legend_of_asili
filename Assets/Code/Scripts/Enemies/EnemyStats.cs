using UnityEngine;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    // ATTRIBUTES
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _testAmount;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private AudioClip hirtSound; // hit audio



    private float _currentHealth;
    private Animator _animator;
    private int _hashedGotHit = Animator.StringToHash("GotHit");


    // STARTUP
    private void Start()
    {
        _animator = GetComponent<Animator>();
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
        _animator.SetTrigger(_hashedGotHit);
        if (hirtSound != null)
        {
            SoundManager.Instance.PlaySound(hirtSound);
        }
        
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
        Destroy(gameObject); // destroy enemy from game scene
    }

    public void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo != null && hitInfo.CompareTag("Enemy"))
        {
            TakeDamage(_testAmount);
        }
    }
}
