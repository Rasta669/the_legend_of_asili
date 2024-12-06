using UnityEngine;
using UnityEngine.SceneManagement;

public class TempEndgameQuit : MonoBehaviour
{

    public void GotoMainMenu()
    {
        // Return to the main UI or quit the application
        SceneManager.LoadScene(1);
    }
}
