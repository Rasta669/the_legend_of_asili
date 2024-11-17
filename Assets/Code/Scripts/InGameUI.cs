using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject m_PausedCanvas;
    [SerializeField] private GameObject m_ActiveCanvas;

    // [SerializeField] private Texture2D m_CustomCursor;

    void Start()
    {
        // Temp cursor settings
        // if (m_CustomCursor != null)
        // {   
        //     Cursor.SetCursor(m_CustomCursor, Vector2.zero, CursorMode.Auto);
        //     Cursor.lockState = CursorLockMode.Locked;

        // }
        m_PausedCanvas.SetActive(false);
        m_ActiveCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        m_PausedCanvas.SetActive(false);
        m_ActiveCanvas.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        m_PausedCanvas.SetActive(true);
        m_ActiveCanvas.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("UIScene");
    }
}
