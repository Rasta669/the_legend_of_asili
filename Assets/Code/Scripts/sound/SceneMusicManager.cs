using UnityEngine;

public class SceneMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip sceneMusic;

    private void Start()
    {
        if (SoundManager.instance != null && sceneMusic != null)
        {
            SoundManager.instance.ChangeMusic(sceneMusic);
        }
    }
}
