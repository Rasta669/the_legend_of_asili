using UnityEngine;
using TMPro;

public class Portal : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameObject _currentLevel; // reference the current level
    [SerializeField] private GameObject _prevLevel;
    [SerializeField] private GameObject _nextPortal; // Reference to the next portal
    [SerializeField] private Transform _nextSpawnLocation; // new spawn point
    [SerializeField] public GameObject _nextLevel; // reference entire next level
    [SerializeField] public bool _isLastPortal;    // Check if this is the last portal
    [SerializeField] private Transform _gemGroup;  // Parent object containing gems for this level
    [SerializeField] private TextMeshProUGUI _gemCountText; // text ui to display gems collected

    private GameObject _player;
    private int _gemsToCollect;       // Total gems to collect
    private int _gemsCollected = 0;  // Gems collected so far

    public void Start()
    {
        if (_nextLevel != null) { _nextLevel.SetActive(false); } // disable next level

        _player = GameObject.FindWithTag("Player");

        // Dynamically retrieve all gem children from the gem group
        if (_gemGroup != null)
        {
            foreach (Transform gem in _gemGroup)
            {
                if (gem.gameObject.activeSelf)
                {
                    Debug.Log($"Adding gem: {gem.gameObject.name} to collections.");
                    _gemsToCollect++;
                }
            }

            UpdateGemUI(); // Update the UI at the start
        }
        else
        {
            Debug.LogWarning("No Gem Group assigned to the Portal!");
        }
    }

    private void Update()
    {
        if (_gemsCollected == _gemsToCollect)
        {
            _nextLevel.SetActive(true); // Activate the next portal
        }
    }
    private void UpdateGemUI()
    {
        // Update the UI to show collected gems out of total gems
        _gemCountText.text = $"{_gemsCollected} / {_gemsToCollect}";
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
            Debug.Log($"Portal Open! Moving to next area: {_nextPortal.name}");            // teleport player to new level
            if (_nextSpawnLocation)
            {
                TeleportPlayer(_nextSpawnLocation.position);
            }
        }

        // disable current level;
        Invoke(nameof(RidOldLevel), 5f);
    }

    private void RidOldLevel()
    {
        if (_prevLevel != null)
        {
            // _prevLevel.SetActive(false);
            DestroyImmediate(_prevLevel);
        }
    }

    private void TeleportPlayer(Vector3 destination)
    {
        if (_player != null)
        {
            _player.transform.position = destination; // set player location to new designated location
            Debug.Log($"Player Teleported to: {destination}");
        }
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
