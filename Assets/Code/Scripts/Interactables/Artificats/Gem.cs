using UnityEngine;

public class Gem : MonoBehaviour, IInteractable
{
    private Animator _gemAnimator; // Reference to the animator for effects
    private bool _hasPickedUpGem = false;

    private void Start()
    {
        _gemAnimator = GetComponent<Animator>();
    }
    public void Interact()
    {
        if (!_hasPickedUpGem)
        {
            GemsManager.Instance.GemCollected();
            _hasPickedUpGem = true;
            Invoke(nameof(GetRidOfObject), 1f);
        }
        else
        {
            return;
        }
    }
    private void GetRidOfObject()
    {
        Destroy(gameObject);
        _hasPickedUpGem = false;
    }

    public void OnPlayerApproach()
    {

        if (_gemAnimator != null)
        {
            Debug.Log("Player is near the gem!");
            _gemAnimator.SetBool("IsNear", true); // Stop animation
        }
    }

    public void OnPlayerLeave()
    {
        if (_gemAnimator != null)
        {
            Debug.Log("Player left the gem!");
            _gemAnimator.SetBool("IsNear", false); // Stop animation
        }
    }
}
