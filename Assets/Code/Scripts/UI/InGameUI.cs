using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject m_PausedCanvas;
    [SerializeField] private GameObject m_ActiveCanvas;

    void Start()
    {
        m_PausedCanvas.SetActive(false);
        m_ActiveCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        // Ensure UI is active
        m_PausedCanvas.SetActive(false);
        m_ActiveCanvas.SetActive(true);

        // Reset game time
        Time.timeScale = 1.0f;

        // Ensure game logic and physics are resumed
        // You can also disable certain game objects manually here if needed
    }

    public void PauseGame()
    {
        // Show paused UI and disable active gameplay UI
        m_PausedCanvas.SetActive(true);
        m_ActiveCanvas.SetActive(false);

        // Freeze the game mechanics but allow UI input
        Time.timeScale = 0.0f;

        // Allow UI interactions even when time scale is zero
        // You could also use this for controlling animations or UI-specific elements
        // Disable other gameplay logic manually as needed (e.g., stop moving objects, etc.)
    }

    public void QuitGame()
    {
        // Return to the main UI or quit the application
        Time.timeScale = 1.0f;  // Ensure time is resumed
        SceneManager.LoadScene(0);
    }
}
