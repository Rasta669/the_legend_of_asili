using UnityEngine;
using TMPro;

public class GemsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gemsCollectedText;

    private int _gemsToCollect = 0;
    public int GemsToCollect
    {
        get => _gemsToCollect;
        private set
        {
            _gemsToCollect = value;
            UpdateGemUI(); // Ensure UI updates when GemsToCollect changes
        }
    }

    private int _gemsCollected = 0;
    public int GemsCollected
    {
        get => _gemsCollected;
        private set
        {
            _gemsCollected = value;
            UpdateGemUI(); // Ensure UI updates when GemsCollected changes
        }
    }

    public static GemsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateGemUI();
    }

    private void UpdateGemUI()
    {
        if (_gemsCollectedText != null)
        {
            _gemsCollectedText.text = $"{GemsCollected} / {GemsToCollect}";
        }
    }

    public void SetMaxGems(int newMax)
    {
        GemsToCollect = GemsCollected + newMax; // Update the property, not the field
    }

    public void GemCollected()
    {
        GemsCollected++;
    }

    public void CanActivatePortalFeedBack(GameObject portal)
    {
        Debug.Log($"All Gems have been collected: {portal.name} can be activated");
    }

    public void CannotActivatePortalFeedBack(GameObject portal)
    {
        Debug.Log($"Sorry, Collect all Gems First to activate {portal.name}");
    }
}
