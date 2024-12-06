using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject m_PausedCanvas;
    [SerializeField] private GameObject m_ActiveCanvas;
    private bool isPaused = false;

    void Start()
    {
        m_PausedCanvas.SetActive(false);
        m_ActiveCanvas.SetActive(true);

        // lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Check for the pause input (example: Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Pause game logic, but allow UI to interact
            PauseGame();
        }
        else
        {
            // Resume game logic, but allow UI to interact
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        // Ensure UI is active
        m_PausedCanvas.SetActive(false);
        m_ActiveCanvas.SetActive(true);

        // Reset game time
        Time.timeScale = 1.0f;

        // Hide and lock the cursor when unpaused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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


        // Show the cursor and unlock it when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        // Allow UI interactions even when time scale is zero
        // You could also use this for controlling animations or UI-specific elements
        // Disable other gameplay logic manually as needed (e.g., stop moving objects, etc.)
    }

    public void QuitGame()
    {
        // Return to the main UI or quit the application
        Time.timeScale = 1.0f;  // Ensure time is resumed
        SceneManager.LoadScene(1);
    }
}
