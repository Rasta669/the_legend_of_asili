using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundStory : MonoBehaviour
{
    public float delay = 10f; // Time in seconds before transitioning

    void Start()
    {
        StartCoroutine(LoadStartMenuAfterDelay());
    }

    IEnumerator LoadStartMenuAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("StartMenuScene"); // Replace with your Start Menu scene name
    }

    public void SkipStory()
    {
        SceneManager.LoadScene("StartMenuScene"); // Replace with your Start Menu scene name
    }
}
