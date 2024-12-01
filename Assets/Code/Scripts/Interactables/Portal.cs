using UnityEngine;
using TMPro;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _nextPortal; // Reference to the next portal
    [SerializeField] private bool _isLastPortal;    // Check if this is the last portal
    [SerializeField] private Transform _gemGroup;  // Parent object containing gems for this level

    private int _gemsToCollect;       // Total gems to collect
    private int _gemsCollected = 0;  // Gems collected so far
    [SerializeField] private TextMeshProUGUI _gemsUI;

    public void Start()
    {
        // Dynamically retrieve all gem children from the gem group
        if (_gemGroup != null)
        {
            foreach (Transform child in _gemGroup)
            {
                if (child.gameObject.activeSelf)
                {
                    Debug.Log($"Adding gem: {child.gameObject.name} to collections.");
                    _gemsToCollect++;
                }
            }

            UpdateGemUI(); // Update the UI at the start
        }
        else
        {
            Debug.LogWarning("No Gem Group assigned to the Portal!");
        }

        if (_nextPortal != null) { _nextPortal.SetActive(false); }
    }

    private void UpdateGemUI()
    {
        // Update the UI to show collected gems out of total gems
        _gemsUI.text = $"{_gemsCollected} / {_gemsToCollect}";
    }

    public void Interact()
    {
        if (_gemsCollected >= _gemsToCollect)
        {
            ActivatePortal();
        }
        else
        {
            Debug.Log($"Portal locked. {_gemsToCollect - _gemsCollected} gem(s) still need to be collected!");
        }
    }

    private void ActivatePortal()
    {
        Debug.Log("Interacting with portal...");
        if (_isLastPortal)
        {
            Debug.Log("This is the last portal...");
            // Trigger end-game sequence or victory screen here
        }
        else if (_nextPortal != null)
        {
            Debug.Log($"Portal Open! Moving to next area: {_nextPortal.name}");
            _nextPortal.SetActive(true); // Activate the next portal
        }
        Destroy(gameObject);
    }

    public void OnPlayerApproach()
    {
        Debug.Log("Approaching the portal...");
        if (_gemsCollected < _gemsToCollect)
        {
            Debug.Log($"Collect all gems to activate this portal. {_gemsToCollect - _gemsCollected} remaining.");
        }
    }

    public void OnPlayerLeave()
    {
        Debug.Log("Leaving the portal...");
    }

    public void GemCollected()
    {
        _gemsCollected++;
        Debug.Log($"Gem collected! {_gemsCollected} / {_gemsToCollect}");

        UpdateGemUI(); // Update the UI whenever a gem is collected

        if (_gemsCollected >= _gemsToCollect)
        {
            Debug.Log("All gems collected! Portal can now be activated.");
            // Additional visual or audio feedback to indicate portal is ready
        }
    }
}
