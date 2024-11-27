using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable
{
    public Animator weaponAnimator; // Reference to the animator for effects

    public void Interact()
    {
        Debug.Log("Picked up the weapon!");
        Destroy(gameObject);
    }

    public void OnPlayerApproach()
    {
        Debug.Log("Player is near the weapon!");
        weaponAnimator.SetBool("IsNear", true); // Trigger animation
    }

    public void OnPlayerLeave()
    {
        Debug.Log("Player left the weapon!");
        weaponAnimator.SetBool("IsNear", false); // Stop animation
    }
}
