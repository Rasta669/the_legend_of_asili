using UnityEngine;

public class Gem : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator _gemAnimator; // Reference to the animator for effects
    [SerializeField] private Portal _portal; // reference to gem owning portal
    public void Interact()
    {
        Debug.Log("Collected the gem!");
        _portal.GemCollected();
        Destroy(gameObject);
    }

    public void OnPlayerApproach()
    {
        Debug.Log("Player is near the gem!");
        // Add visual or sound effects here
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
