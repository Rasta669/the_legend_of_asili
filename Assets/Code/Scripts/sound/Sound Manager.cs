using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource musicSource, effectsSource;
    [SerializeField] public AudioSource playerFootStepSource; // players footsteps
    [SerializeField] public AudioSource enemyFootStepSource; // enemy footsteps
    [SerializeField] public AudioSource interactableAudioSource; // enemy footsteps

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //musicSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this;
            //musicSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            Debug.Log("SoundManager initialized");
        }
        // else
        // {
        //    Debug.LogWarning("Duplicate SoundManager destroyed");
        //    Destroy(gameObject);
        // }
    }


    private void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        if (musicSource == null)
        {
            Debug.LogError("musicSource is not assigned or initialized!");
            return;
        }

        if (!musicSource.isPlaying)
        {
            Debug.Log("MusicSource assigned: " + musicSource.name);
            musicSource.Play();
            Debug.Log("Music Playing");
        }
        else
        {
            Debug.Log("Music already playing");
        }

        if (musicSource.clip == null)
        {
            Debug.LogError("No audio clip is assigned to the musicSource!");
        }
        else
        {
            Debug.Log("Assigned clip: " + musicSource.clip.name);
        }
    }


    public void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }
    public void PlayPortalSound(AudioClip clip)
    {
        interactableAudioSource.PlayOneShot(clip);
    }

    public void PlayerFootStepSound(AudioClip _audioClip)
    {
        if (playerFootStepSource != null)
        {

            playerFootStepSource.PlayOneShot(_audioClip);
        }
    }
    public void EnemyFootStepSound(AudioClip _audioClip)
    {

        if (enemyFootStepSource != null)
        {
            enemyFootStepSource.PlayOneShot(_audioClip);
        }
    }

    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = !musicSource.mute;
            Debug.Log("Music toggled. Mute status: " + musicSource.mute);
        }
        else
        {
            Debug.LogError("MusicSource is not assigned!");
        }
    }

    public void ToggleEffects()
    {
        effectsSource.mute = !effectsSource.mute;

    }
    private void Update()
    {
        // check scene and change music clip
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
            Debug.Log("Music stopped as scene was unloaded.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public IEnumerator FadeAndChangeMusic(AudioClip newClip, float fadeDuration)
    {
        if (musicSource != null)
        {
            // Fade out
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }

            musicSource.Stop();
            musicSource.clip = newClip;
            musicSource.Play();

            // Fade in
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (musicSource != null)
        {
            if (musicSource.clip != newClip)
            {
                musicSource.Stop();
                musicSource.clip = newClip;
                musicSource.Play();
                Debug.Log("Music changed to: " + newClip.name);
            }
            else
            {
                Debug.Log("The new clip is the same as the current one. No change made.");
            }
        }
        else
        {
            Debug.LogError("MusicSource is not assigned!");
        }
    }

}
