using UnityEngine;

public class ButtonSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;

    public void OnButtonClick()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlaySound(buttonClickSound);
    }
}

