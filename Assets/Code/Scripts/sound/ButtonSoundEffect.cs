using UnityEngine;

public class ButtonSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;

    public void OnButtonClick()
    {
        SoundManager.Instance.PlaySound(buttonClickSound);

        // Add additional logic for the button here
    }
}

