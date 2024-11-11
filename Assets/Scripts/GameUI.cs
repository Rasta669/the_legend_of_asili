using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject startCanvas;
    //InGameUI gameUI;
    //[SerializeField] SceneManager sceneManager;
    void Start()
    {
        startCanvas.SetActive(true);
        //gameUI = GameObject.Find("inGameUI").GetComponent<InGameUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TestArea");
        startCanvas.SetActive(false); 
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
