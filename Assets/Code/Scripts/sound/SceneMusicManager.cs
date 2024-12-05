using UnityEngine;

public class SceneMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip sceneMusic;

    private void Start()
    {
        if (SoundManager.Instance != null && sceneMusic != null)
        {
            SoundManager.Instance.ChangeMusic(sceneMusic);   
        }
    }
}
