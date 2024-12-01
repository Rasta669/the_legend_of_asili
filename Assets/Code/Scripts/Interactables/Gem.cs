using UnityEngine;

public class Gem : MonoBehaviour, IInteractable
{
    [SerializeField] private Portal _portal; // Reference to the gem's owning portal
    private Animator _gemAnimator; // Reference to the animator for effects


    private void Start()
    {
        _gemAnimator = GetComponent<Animator>();
    }
    public void Interact()
    {
        Debug.Log("Collected the gem!");
        _portal.GemCollected(); // Notify the portal that a gem has been collected
        Destroy(gameObject);    // Remove the gem from the scene
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
