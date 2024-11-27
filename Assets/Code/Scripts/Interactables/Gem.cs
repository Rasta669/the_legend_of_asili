using UnityEngine;

public class Gem : MonoBehaviour, IInteractable
{
    public Animator gemAnimator; // Reference to the animator for effects

    public void Interact()
    {
        Debug.Log("Collected the gem!");
        GetComponent<Collider>().enabled = false; // Disable collider to prevent further checks
        Destroy(gameObject); // Destroy the gem
    }

    public void OnPlayerApproach()
    {
        Debug.Log("Player is near the gem!");
        gemAnimator.SetBool("IsNear", true); // Trigger animation
    }

    public void OnPlayerLeave()
    {
        if (gemAnimator != null)
        {
            Debug.Log("Player left the gem!");
            gemAnimator.SetBool("IsNear", false); // Stop animation
        }
    }
}
