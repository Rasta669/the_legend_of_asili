using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GemsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gemsCollectedText;
    [SerializeField] private GameObject _canvas; // Reference to the UI canvas or parent GameObject

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

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

   // Toggle the visibility of the canvas based on the scene name
    private void ToggleCanvasVisibility(string sceneName)
    {
        if (_canvas != null)
        {
            _canvas.SetActive(sceneName == "LevelDesign"); // Show only in "LevelDesign"
        }
    }
    // Reset the gem count when the scene reloads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        // Check if the canvas should be visible
        ToggleCanvasVisibility(scene.name);


        _gemsToCollect = 0;   // Reset total gems to collect
        _gemsCollected = 0;   // Reset collected gems
        UpdateGemUI();        // Update the UI to reflect the reset state
    }
}
