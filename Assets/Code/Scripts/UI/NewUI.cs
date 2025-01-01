using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class NewUI : MonoBehaviour
{
    UIDocument UIDocument;
    Button newGAme;
    Button Options;
    Button Quit;
    VisualElement BaseMenu;
    VisualElement OptionsMenu;
    Button Audio;
    Button Video;
    Button Gameplay;
    Button Controls;
    Button Language;
    VisualElement AudioMenu;
    VisualElement VideoMenu;
    //VisualElement GameplayMenu;
    VisualElement LanguageMenu;

    private void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
        newGAme = UIDocument.rootVisualElement.Q<Button>("NewGameButton");
        Options = UIDocument.rootVisualElement.Q<Button>("OptionsButton");
        Quit = UIDocument.rootVisualElement.Q<Button>("QuitButton");
        BaseMenu = UIDocument.rootVisualElement.Q<VisualElement>("BaseMenu");
        OptionsMenu = UIDocument.rootVisualElement.Q<VisualElement>("OptionsMenu");
        Audio = UIDocument.rootVisualElement.Q<Button>("AudioButton");
        Video = UIDocument.rootVisualElement.Q<Button>("VideoButton");
        Gameplay = UIDocument.rootVisualElement.Q<Button>("GameplayButton");
        Controls = UIDocument.rootVisualElement.Q<Button>("ControlsButton");
        Language = UIDocument.rootVisualElement.Q<Button>("LanguageButton");
        AudioMenu = UIDocument.rootVisualElement.Q<VisualElement>("AudioMenu");
        LanguageMenu = UIDocument.rootVisualElement.Q<VisualElement>("LanguageMenu");
        VideoMenu = UIDocument.rootVisualElement.Q<VisualElement>("VideoMenu");

    }

    void Start()
    {
        newGAme.RegisterCallback<ClickEvent>(NewGame);
        Options.RegisterCallback<ClickEvent>(GameOptions);
        Quit.RegisterCallback<ClickEvent>(QuitGame);
        Audio.RegisterCallback<ClickEvent>(GameAudio);
        Video.RegisterCallback<ClickEvent>(GameVideo);
        Controls.RegisterCallback<ClickEvent>(GameControls);
        Language.RegisterCallback<ClickEvent>(GameLanguage);
        Gameplay.RegisterCallback<ClickEvent>(GamePLay);
        BaseMenu.visible = true;
        OptionsMenu.visible = false;
        AudioMenu.visible = false;
        VideoMenu.visible = false;
        LanguageMenu.visible = false;
    }

    void NewGame(ClickEvent clk)
    {
        // Load a new scene here
        Debug.Log("New game!");
    }

    void GameOptions(ClickEvent clk)
    {
        Debug.Log("Options!");
        BaseMenu.visible = false;
        OptionsMenu.visible = true;
    }

    void QuitGame(ClickEvent clk)
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    void GameAudio(ClickEvent clk) {
        Debug.Log("Audio...");
        AudioMenu.visible = true;
        VideoMenu.visible = false;
        LanguageMenu.visible = false;

    }

    void GameVideo(ClickEvent clk) {
        Debug.Log("Video...");
        AudioMenu.visible = false;
        VideoMenu.visible = true;
        LanguageMenu.visible = false;
    }

    void GameControls(ClickEvent clk) {
        Debug.Log("Controls...");
    }

    void GameLanguage(ClickEvent clk) {
        Debug.Log("Language...");
        AudioMenu.visible = false;
        VideoMenu.visible = false;
        LanguageMenu.visible = true;
    }

    void GamePLay(ClickEvent lk) {
        Debug.Log("Gameplay...");
    }
}
