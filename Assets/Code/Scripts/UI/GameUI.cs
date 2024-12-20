using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject startCanvas;
   
    void Start()
    {
        startCanvas.SetActive(true);
    }

    public void StartGame()
    {
        // SceneManager.LoadScene("TestArea");
        startCanvas.SetActive(false); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

   

}


