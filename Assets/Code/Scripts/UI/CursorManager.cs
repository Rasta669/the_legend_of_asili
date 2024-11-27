using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private InGameUI _inGameUI;
    [SerializeField] private Texture2D _cursor;

    private bool isPaused = false;

    private void Start()
    {
        _inGameUI = GetComponent<InGameUI>();
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (_inGameUI != null)
        {
            // Check for the pause input (example: Escape key)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Pause game logic, but allow UI to interact
            _inGameUI.PauseGame();
            
            // Show the cursor and unlock it when paused
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Resume game logic, but allow UI to interact
            _inGameUI.ResumeGame();

            // Hide and lock the cursor when unpaused
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
