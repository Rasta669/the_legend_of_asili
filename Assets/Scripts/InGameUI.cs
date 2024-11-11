using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public GameObject pausedCanvas;
    public GameObject activeCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausedCanvas.SetActive(false);
        activeCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame() { 
        pausedCanvas.gameObject.SetActive(false);
        activeCanvas.gameObject.SetActive(true);
        Time.timeScale = 1.0f;   
    }

    public void PauseGame()
    {
        pausedCanvas.gameObject.SetActive(true);
        activeCanvas.gameObject.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("UIScene");
    }
}
