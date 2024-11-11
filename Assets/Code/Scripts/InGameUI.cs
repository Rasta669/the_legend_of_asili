using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public GameObject pausedCanvas;
    public GameObject activeCanvas;

    void Start()
    {
        pausedCanvas.SetActive(false);
        activeCanvas.SetActive(true);
    }

    public void ResumeGame() { 
        pausedCanvas.SetActive(false);
        activeCanvas.SetActive(true);
        Time.timeScale = 1.0f;   
    }

    public void PauseGame()
    {
        pausedCanvas.SetActive(true);
        activeCanvas.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("UIScene");
    }
}
