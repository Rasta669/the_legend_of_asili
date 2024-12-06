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
    private CharacterController _characterController;
    private int _hashedGotHit = Animator.StringToHash("GotHit");
    private int _hashedDeath = Animator.StringToHash("Death");

    // Add this flag to track whether the enemy is dead
    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _currentHealth = _maxHealth;

        if (_healthBar != null)
        {
            _healthBar.SetSliderMax(_maxHealth);
        }
    }

    private void Update()
    {
        DisplayHealth();
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        if (_currentHealth <= 0 && !IsDead) // Check if already dead to avoid multiple triggers
        {
            _currentHealth = 0;
            HandleCharacterDeath();
        }
    }

    private void HandleCharacterDeath()
    {
        IsDead = true; // Set the flag
        _characterController.enabled = false;
        _animator.SetTrigger(_hashedDeath);
        Debug.Log("Enemy is dead...");
        Invoke(nameof(MeshCleanUp), 3f); // Delay for clean-up
    }

    private void DisplayHealth()
    {
        _healthText.text = $"{_currentHealth} / {_maxHealth}";
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return; // Do nothing if already dead

        _animator.SetTrigger(_hashedGotHit);
        if (hirtSound != null)
        {
            SoundManager.Instance.PlaySound(hirtSound);
        }

        _currentHealth -= amount;
        _healthBar.SetSlider(_currentHealth);
    }

    private void MeshCleanUp()
    {
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
