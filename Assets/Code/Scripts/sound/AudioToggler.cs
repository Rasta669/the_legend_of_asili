using UnityEngine;
using UnityEngine.UI;

public class AudioToggler : MonoBehaviour
{
    [SerializeField] private bool toggleMusic, toggleEffects; // Flags for toggle type
    [SerializeField] private Sprite[] togglerIcons; // Array of images to toggle between (2 elements for each toggle)
    [SerializeField] private Image iconHolder; // Reference to the child object's Image component

    private int currentIndex = 0; // Tracks current icon index (0 or 1)

    private void Start()
    {
        // Ensure the icon holder is set
        if (iconHolder == null)
        {
            Debug.LogError("IconHolder is not assigned. Please assign the child Image in the Inspector.");
            return;
        }

        // Set the initial image to index 0
        if (togglerIcons != null && togglerIcons.Length > 0)
        {
            iconHolder.sprite = togglerIcons[0];
        }
        else
        {
            Debug.LogWarning("Toggler icons not properly set! Ensure you have at least 2 icons assigned.");
        }
    }

    public void Toggle()
    {
        // Switch the current index (0 -> 1 or 1 -> 0)
        currentIndex = (currentIndex == 0) ? 1 : 0;

        // Update the child icon's Image sprite
        if (iconHolder != null && togglerIcons != null && togglerIcons.Length == 2)
        {
            iconHolder.sprite = togglerIcons[currentIndex];
        }
        else
        {
            Debug.LogWarning("IconHolder or Toggler icons are not properly set! Ensure you have the child Image assigned and exactly 2 icons in the array.");
        }

        // Perform additional toggle behavior if needed
        if (toggleMusic)
        {
            Debug.Log("Music toggle clicked");
            SoundManager.Instance.ToggleMusic();
        }

        if (toggleEffects)
        {
            Debug.Log("Effects toggle clicked");
            SoundManager.Instance.ToggleEffects();
        }
    }
}
