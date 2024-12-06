using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private bool toggleMusic, toggleEffects;


    public void Toggle()
    {
        if (toggleMusic)
        {
            Debug.Log("Toggling music");
            SoundManager.Instance.ToggleMusic();
        }
        if (toggleEffects)
        {
            Debug.Log("Toggling effects");
            SoundManager.Instance.ToggleEffects();  
        }
    }
}
