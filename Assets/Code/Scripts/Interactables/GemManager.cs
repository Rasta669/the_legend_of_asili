using UnityEngine;
using TMPro;

public class GemsManager : MonoBehaviour
{
    public static GemsManager Instance { get; private set; }

    private int _totalGemsCollected = 0; // Total gems collected across all levels
    private int _totalGemsRequired = 0;  // Total gems required across all levels

    [SerializeField] private TextMeshProUGUI _gemCountText; // Gem UI text

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes if needed
    }

    public void AddLevelGems(int gemsForLevel)
    {
        _totalGemsRequired += gemsForLevel;
        UpdateGemUI();
    }

    public void CollectGem()
    {
        _totalGemsCollected++;
        UpdateGemUI();
    }

    private void UpdateGemUI()
    {
        _gemCountText.text = $"{_totalGemsCollected} / {_totalGemsRequired}";
    }
}
