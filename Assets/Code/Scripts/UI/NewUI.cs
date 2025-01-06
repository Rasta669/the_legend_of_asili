//using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEngine.SceneManagement;

//public class NewUI : MonoBehaviour
//{
//    UIDocument UIDocument;
//    Button newGAme;
//    Button Options;
//    Button Quit;
//    VisualElement BaseMenu;
//    VisualElement OptionsMenu;
//    Button Audio;
//    Button Video;
//    Button Gameplay;
//    Button Controls;
//    Button Language;
//    VisualElement AudioMenu;
//    VisualElement VideoMenu;
//    VisualElement LanguageMenu;

//    private VisualElement[] currentInteractables;
//    private VisualElement[] baseInteractables;
//    private VisualElement[] optionInteractables;
//    private int currentInteractableIndex = 0;

//    // sliders
//    Slider musicVolume;
//    Slider masterVolume;
//    Slider effectsVolume;
//    Slider voiceVolume;

//    private Slider[] sliders;

//    private void Awake()
//    {
//        UIDocument = GetComponent<UIDocument>();
//        newGAme = UIDocument.rootVisualElement.Q<Button>("NewGameButton");
//        Options = UIDocument.rootVisualElement.Q<Button>("OptionsButton");
//        Quit = UIDocument.rootVisualElement.Q<Button>("QuitButton");
//        BaseMenu = UIDocument.rootVisualElement.Q<VisualElement>("BaseMenu");
//        OptionsMenu = UIDocument.rootVisualElement.Q<VisualElement>("OptionsMenu");
//        Audio = UIDocument.rootVisualElement.Q<Button>("AudioButton");
//        Video = UIDocument.rootVisualElement.Q<Button>("VideoButton");
//        Gameplay = UIDocument.rootVisualElement.Q<Button>("GameplayButton");
//        Controls = UIDocument.rootVisualElement.Q<Button>("ControlsButton");
//        Language = UIDocument.rootVisualElement.Q<Button>("LanguageButton");
//        AudioMenu = UIDocument.rootVisualElement.Q<VisualElement>("AudioMenu");
//        LanguageMenu = UIDocument.rootVisualElement.Q<VisualElement>("LanguageMenu");
//        VideoMenu = UIDocument.rootVisualElement.Q<VisualElement>("VideoMenu");

//        masterVolume = UIDocument.rootVisualElement.Q<Slider>("MasterVolume");
//        effectsVolume = UIDocument.rootVisualElement.Q<Slider>("EffectsVolume");
//        musicVolume = UIDocument.rootVisualElement.Q<Slider>("MusicVolume");
//        voiceVolume = UIDocument.rootVisualElement.Q<Slider>("VoiceVolume");

//        // Initialize the arrays of interactables (buttons + sliders)
//        baseInteractables = new VisualElement[] { newGAme, Options, Quit };
//        optionInteractables = new VisualElement[] { Audio, Controls, Gameplay, Video, Language };
//        sliders = new Slider[] { masterVolume, musicVolume, effectsVolume, voiceVolume };

//        currentInteractables = baseInteractables;
//    }

//    void Start()
//    {
//        newGAme.RegisterCallback<ClickEvent>(NewGame);
//        Options.RegisterCallback<ClickEvent>(GameOptions);
//        Quit.RegisterCallback<ClickEvent>(QuitGame);
//        Audio.RegisterCallback<ClickEvent>(GameAudio);
//        Video.RegisterCallback<ClickEvent>(GameVideo);
//        Controls.RegisterCallback<ClickEvent>(GameControls);
//        Language.RegisterCallback<ClickEvent>(GameLanguage);
//        Gameplay.RegisterCallback<ClickEvent>(GamePLay);

//        BaseMenu.visible = true;
//        OptionsMenu.visible = false;
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;

//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void Update()
//    {
//        HandleKeyboardNavigation();
//    }

//    void HandleKeyboardNavigation()
//    {
//        if (Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            NavigateToPreviousInteractable();
//        }
//        else if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            NavigateToNextInteractable();
//        }
//        else if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            AdjustSliderValue(-1);
//        }
//        else if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            AdjustSliderValue(1);
//        }
//        else if (Input.GetKeyDown(KeyCode.Return))
//        {
//            TriggerInteractableAction(currentInteractables[currentInteractableIndex]);
//        }
//    }

//    void NavigateToPreviousInteractable()
//    {
//        currentInteractableIndex = (currentInteractableIndex - 1 + currentInteractables.Length) % currentInteractables.Length;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void NavigateToNextInteractable()
//    {
//        currentInteractableIndex = (currentInteractableIndex + 1) % currentInteractables.Length;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void HighlightInteractable(VisualElement interactable)
//    {
//        ResetInteractableStyles();

//        if (interactable is Button button)
//        {
//            button.style.borderTopWidth = 12;
//            button.style.borderRightWidth = 12;
//            button.style.borderBottomWidth = 12;
//            button.style.borderLeftWidth = 12;
//        }
//        else if (interactable is Slider slider)
//        {
//            slider.style.borderTopWidth = 12;
//            slider.style.borderRightWidth = 12;
//            slider.style.borderBottomWidth = 12;
//            slider.style.borderLeftWidth = 12;
//        }
//    }

//    void ResetInteractableStyles()
//    {
//        foreach (var interactable in currentInteractables)
//        {
//            if (interactable is Button button)
//            {
//                button.style.borderTopWidth = 0;
//                button.style.borderRightWidth = 0;
//                button.style.borderBottomWidth = 0;
//                button.style.borderLeftWidth = 0;
//            }
//            else if (interactable is Slider slider)
//            {
//                slider.style.borderTopWidth = 0;
//                slider.style.borderRightWidth = 0;
//                slider.style.borderBottomWidth = 0;
//                slider.style.borderLeftWidth = 0;
//            }
//        }
//    }

//    void AdjustSliderValue(int direction)
//    {
//        if (currentInteractables[currentInteractableIndex] is Slider slider)
//        {
//            slider.value += direction * 0.1f;
//            slider.value = Mathf.Clamp(slider.value, slider.lowValue, slider.highValue);
//        }
//    }

//    void TriggerInteractableAction(VisualElement interactable)
//    {
//        if (interactable is Button button)
//        {
//            if (button == newGAme) NewGame(null);
//            else if (button == Options) GameOptions(null);
//            else if (button == Quit) QuitGame(null);
//            else if (button == Audio) GameAudio(null);
//            else if (button == Video) GameVideo(null);
//            else if (button == Gameplay) GamePLay(null);
//            else if (button == Controls) GameControls(null);
//            else if (button == Language) GameLanguage(null);
//        }
//    }

//    void NewGame(ClickEvent clk)
//    {
//        Debug.Log("New game!");
//    }

//    void GameOptions(ClickEvent clk)
//    {
//        Debug.Log("Options!");
//        BaseMenu.visible = false;
//        OptionsMenu.visible = true;
//        currentInteractables = optionInteractables;
//        currentInteractableIndex = 0;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void QuitGame(ClickEvent clk)
//    {
//        Debug.Log("Quit!");
//        Application.Quit();
//    }

//    void GameAudio(ClickEvent clk)
//    {
//        Debug.Log("Audio...");
//        currentInteractables = sliders;
//        currentInteractableIndex = 0;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);

//        AudioMenu.visible = true;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;
//    }

//    void GameVideo(ClickEvent clk)
//    {
//        Debug.Log("Video...");
//        AudioMenu.visible = false;
//        VideoMenu.visible = true;
//        LanguageMenu.visible = false;
//    }

//    void GameControls(ClickEvent clk)
//    {
//        Debug.Log("Controls...");
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;
//    }

//    void GameLanguage(ClickEvent clk)
//    {
//        Debug.Log("Language...");
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = true;
//    }

//    void GamePLay(ClickEvent clk)
//    {
//        Debug.Log("Gameplay...");
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;
//    }
//}


//using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEngine.SceneManagement;

//public class NewUI : MonoBehaviour
//{
//    UIDocument UIDocument;
//    Button newGAme;
//    Button Options;
//    Button Quit;
//    VisualElement BaseMenu;
//    VisualElement OptionsMenu;
//    Button Audio;
//    Button Video;
//    Button Gameplay;
//    Button Controls;
//    Button Language;
//    VisualElement AudioMenu;
//    VisualElement VideoMenu;
//    VisualElement LanguageMenu;

//    private VisualElement[] currentInteractables;
//    private VisualElement[] baseInteractables;
//    private VisualElement[] optionInteractables;
//    private int currentInteractableIndex = 0;

//    // sliders
//    Slider musicVolume;
//    Slider masterVolume;
//    Slider effectsVolume;
//    Slider voiceVolume;

//    private Slider[] sliders;

//    // Store the previous menu state
//    private VisualElement previousMenu;

//    private void Awake()
//    {
//        UIDocument = GetComponent<UIDocument>();
//        newGAme = UIDocument.rootVisualElement.Q<Button>("NewGameButton");
//        Options = UIDocument.rootVisualElement.Q<Button>("OptionsButton");
//        Quit = UIDocument.rootVisualElement.Q<Button>("QuitButton");
//        BaseMenu = UIDocument.rootVisualElement.Q<VisualElement>("BaseMenu");
//        OptionsMenu = UIDocument.rootVisualElement.Q<VisualElement>("OptionsMenu");
//        Audio = UIDocument.rootVisualElement.Q<Button>("AudioButton");
//        Video = UIDocument.rootVisualElement.Q<Button>("VideoButton");
//        Gameplay = UIDocument.rootVisualElement.Q<Button>("GameplayButton");
//        Controls = UIDocument.rootVisualElement.Q<Button>("ControlsButton");
//        Language = UIDocument.rootVisualElement.Q<Button>("LanguageButton");
//        AudioMenu = UIDocument.rootVisualElement.Q<VisualElement>("AudioMenu");
//        LanguageMenu = UIDocument.rootVisualElement.Q<VisualElement>("LanguageMenu");
//        VideoMenu = UIDocument.rootVisualElement.Q<VisualElement>("VideoMenu");

//        masterVolume = UIDocument.rootVisualElement.Q<Slider>("MasterVolume");
//        effectsVolume = UIDocument.rootVisualElement.Q<Slider>("EffectsVolume");
//        musicVolume = UIDocument.rootVisualElement.Q<Slider>("MusicVolume");
//        voiceVolume = UIDocument.rootVisualElement.Q<Slider>("VoiceVolume");

//        // Initialize the arrays of interactables (buttons + sliders)
//        baseInteractables = new VisualElement[] { newGAme, Options, Quit };
//        optionInteractables = new VisualElement[] { Audio, Controls, Gameplay, Video, Language };
//        sliders = new Slider[] { masterVolume, musicVolume, effectsVolume, voiceVolume };

//        currentInteractables = baseInteractables;
//    }

//    void Start()
//    {
//        newGAme.RegisterCallback<ClickEvent>(NewGame);
//        Options.RegisterCallback<ClickEvent>(GameOptions);
//        Quit.RegisterCallback<ClickEvent>(QuitGame);
//        Audio.RegisterCallback<ClickEvent>(GameAudio);
//        Video.RegisterCallback<ClickEvent>(GameVideo);
//        Controls.RegisterCallback<ClickEvent>(GameControls);
//        Language.RegisterCallback<ClickEvent>(GameLanguage);
//        Gameplay.RegisterCallback<ClickEvent>(GamePLay);

//        BaseMenu.visible = true;
//        OptionsMenu.visible = false;
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;

//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void Update()
//    {
//        HandleKeyboardNavigation();
//    }

//    void HandleKeyboardNavigation()
//    {
//        if (Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            NavigateToPreviousInteractable();
//        }
//        else if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            NavigateToNextInteractable();
//        }
//        else if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            AdjustSliderValue(-1);
//        }
//        else if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            AdjustSliderValue(1);
//        }
//        else if (Input.GetKeyDown(KeyCode.Return))
//        {
//            TriggerInteractableAction(currentInteractables[currentInteractableIndex]);
//        }
//        else if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            RestorePreviousMenu(); // Restore the previous menu when Escape is pressed
//        }
//    }

//    void NavigateToPreviousInteractable()
//    {
//        currentInteractableIndex = (currentInteractableIndex - 1 + currentInteractables.Length) % currentInteractables.Length;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void NavigateToNextInteractable()
//    {
//        currentInteractableIndex = (currentInteractableIndex + 1) % currentInteractables.Length;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void HighlightInteractable(VisualElement interactable)
//    {
//        ResetInteractableStyles();

//        if (interactable is Button button)
//        {
//            button.style.borderTopWidth = 12;
//            button.style.borderRightWidth = 12;
//            button.style.borderBottomWidth = 12;
//            button.style.borderLeftWidth = 12;
//        }
//        else if (interactable is Slider slider)
//        {
//            slider.style.borderTopWidth = 12;
//            slider.style.borderRightWidth = 12;
//            slider.style.borderBottomWidth = 12;
//            slider.style.borderLeftWidth = 12;
//        }
//    }

//    void ResetInteractableStyles()
//    {
//        foreach (var interactable in currentInteractables)
//        {
//            if (interactable is Button button)
//            {
//                button.style.borderTopWidth = 0;
//                button.style.borderRightWidth = 0;
//                button.style.borderBottomWidth = 0;
//                button.style.borderLeftWidth = 0;
//            }
//            else if (interactable is Slider slider)
//            {
//                slider.style.borderTopWidth = 0;
//                slider.style.borderRightWidth = 0;
//                slider.style.borderBottomWidth = 0;
//                slider.style.borderLeftWidth = 0;
//            }
//        }
//    }

//    void AdjustSliderValue(int direction)
//    {
//        if (currentInteractables[currentInteractableIndex] is Slider slider)
//        {
//            slider.value += direction * 0.1f;
//            slider.value = Mathf.Clamp(slider.value, slider.lowValue, slider.highValue);
//        }
//    }

//    void TriggerInteractableAction(VisualElement interactable)
//    {
//        if (interactable is Button button)
//        {
//            if (button == newGAme) NewGame(null);
//            else if (button == Options) GameOptions(null);
//            else if (button == Quit) QuitGame(null);
//            else if (button == Audio) GameAudio(null);
//            else if (button == Video) GameVideo(null);
//            else if (button == Gameplay) GamePLay(null);
//            else if (button == Controls) GameControls(null);
//            else if (button == Language) GameLanguage(null);
//        }
//    }

//    void NewGame(ClickEvent clk)
//    {
//        Debug.Log("New game!");
//    }

//    void GameOptions(ClickEvent clk)
//    {
//        Debug.Log("Options!");
//        BaseMenu.visible = false;
//        OptionsMenu.visible = true;
//        previousMenu = BaseMenu; // Save the current menu state
//        currentInteractables = optionInteractables;
//        currentInteractableIndex = 0;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);
//    }

//    void QuitGame(ClickEvent clk)
//    {
//        Debug.Log("Quit!");
//        Application.Quit();
//    }

//    void GameAudio(ClickEvent clk)
//    {
//        Debug.Log("Audio...");
//        previousMenu = OptionsMenu; // Save the current menu state
//        currentInteractables = sliders;
//        currentInteractableIndex = 0;
//        HighlightInteractable(currentInteractables[currentInteractableIndex]);

//        AudioMenu.visible = true;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;
//    }

//    void GameVideo(ClickEvent clk)
//    {
//        Debug.Log("Video...");
//        previousMenu = OptionsMenu; // Save the current menu state
//        AudioMenu.visible = false;
//        VideoMenu.visible = true;
//        LanguageMenu.visible = false;
//    }

//    void GameControls(ClickEvent clk)
//    {
//        Debug.Log("Controls...");
//        previousMenu = OptionsMenu; // Save the current menu state
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;
//    }

//    void GameLanguage(ClickEvent clk)
//    {
//        Debug.Log("Language...");
//        previousMenu = OptionsMenu; // Save the current menu state
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = true;
//    }

//    void GamePLay(ClickEvent clk)
//    {
//        Debug.Log("Gameplay...");
//        AudioMenu.visible = false;
//        VideoMenu.visible = false;
//        LanguageMenu.visible = false;
//    }

//    void RestorePreviousMenu()
//    {
//        if (previousMenu != null)
//        {
//            // Hide all menus
//            AudioMenu.visible = false;
//            VideoMenu.visible = false;
//            LanguageMenu.visible = false;
//            OptionsMenu.visible = false;

//            // Restore the previous menu
//            previousMenu.visible = true;

//            // Update current interactables based on the previous menu
//            if (previousMenu == BaseMenu)
//            {
//                currentInteractables = baseInteractables;
//            }
//            else if (previousMenu == OptionsMenu)
//            {
//                currentInteractables = optionInteractables;
//            }

//            // Reset the current interactable index to the first element
//            currentInteractableIndex = 0;
//            HighlightInteractable(currentInteractables[currentInteractableIndex]);
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    VisualElement LanguageMenu;

    private VisualElement[] currentInteractables;
    private VisualElement[] baseInteractables;
    private VisualElement[] optionInteractables;
    private int currentInteractableIndex = 0;

    // sliders
    Slider musicVolume;
    Slider masterVolume;
    Slider effectsVolume;
    Slider voiceVolume;

    private Slider[] sliders;

    // Stack to store the previous menu states for undo functionality
    private Stack<VisualElement> menuHistory = new Stack<VisualElement>();

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

        masterVolume = UIDocument.rootVisualElement.Q<Slider>("MasterVolume");
        effectsVolume = UIDocument.rootVisualElement.Q<Slider>("EffectsVolume");
        musicVolume = UIDocument.rootVisualElement.Q<Slider>("MusicVolume");
        voiceVolume = UIDocument.rootVisualElement.Q<Slider>("VoiceVolume");

        // Initialize the arrays of interactables (buttons + sliders)
        baseInteractables = new VisualElement[] { newGAme, Options, Quit };
        optionInteractables = new VisualElement[] { Audio, Controls, Gameplay, Video, Language };
        sliders = new Slider[] { masterVolume, musicVolume, effectsVolume, voiceVolume };

        currentInteractables = baseInteractables;
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

        HighlightInteractable(currentInteractables[currentInteractableIndex]);
    }

    void Update()
    {
        HandleKeyboardNavigation();
    }

    void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateToPreviousInteractable();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateToNextInteractable();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AdjustSliderValue(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AdjustSliderValue(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            TriggerInteractableAction(currentInteractables[currentInteractableIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestorePreviousMenu(); // Restore the previous menu when Escape is pressed
        }
    }

    void NavigateToPreviousInteractable()
    {
        currentInteractableIndex = (currentInteractableIndex - 1 + currentInteractables.Length) % currentInteractables.Length;
        HighlightInteractable(currentInteractables[currentInteractableIndex]);
    }

    void NavigateToNextInteractable()
    {
        currentInteractableIndex = (currentInteractableIndex + 1) % currentInteractables.Length;
        HighlightInteractable(currentInteractables[currentInteractableIndex]);
    }

    void HighlightInteractable(VisualElement interactable)
    {
        ResetInteractableStyles();

        if (interactable is Button button)
        {
            button.style.borderTopWidth = 12;
            button.style.borderRightWidth = 12;
            button.style.borderBottomWidth = 12;
            button.style.borderLeftWidth = 12;
        }
        else if (interactable is Slider slider)
        {
            slider.style.borderTopWidth = 12;
            slider.style.borderRightWidth = 12;
            slider.style.borderBottomWidth = 12;
            slider.style.borderLeftWidth = 12;
        }
    }

    void ResetInteractableStyles()
    {
        foreach (var interactable in currentInteractables)
        {
            if (interactable is Button button)
            {
                button.style.borderTopWidth = 0;
                button.style.borderRightWidth = 0;
                button.style.borderBottomWidth = 0;
                button.style.borderLeftWidth = 0;
            }
            else if (interactable is Slider slider)
            {
                slider.style.borderTopWidth = 0;
                slider.style.borderRightWidth = 0;
                slider.style.borderBottomWidth = 0;
                slider.style.borderLeftWidth = 0;
            }
        }
    }

    void AdjustSliderValue(int direction)
    {
        if (currentInteractables[currentInteractableIndex] is Slider slider)
        {
            slider.value += direction * 0.1f;
            slider.value = Mathf.Clamp(slider.value, slider.lowValue, slider.highValue);
        }
    }

    void TriggerInteractableAction(VisualElement interactable)
    {
        if (interactable is Button button)
        {
            if (button == newGAme) NewGame(null);
            else if (button == Options) GameOptions(null);
            else if (button == Quit) QuitGame(null);
            else if (button == Audio) GameAudio(null);
            else if (button == Video) GameVideo(null);
            else if (button == Gameplay) GamePLay(null);
            else if (button == Controls) GameControls(null);
            else if (button == Language) GameLanguage(null);
        }
    }

    void NewGame(ClickEvent clk)
    {
        Debug.Log("New game!");
    }

    void GameOptions(ClickEvent clk)
    {
        Debug.Log("Options!");
        menuHistory.Push(BaseMenu); // Save the current menu state
        BaseMenu.visible = false;
        OptionsMenu.visible = true;
        currentInteractables = optionInteractables;
        currentInteractableIndex = 0;
        HighlightInteractable(currentInteractables[currentInteractableIndex]);
    }

    void QuitGame(ClickEvent clk)
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    void GameAudio(ClickEvent clk)
    {
        Debug.Log("Audio...");
        menuHistory.Push(OptionsMenu); // Save the current menu state
        currentInteractables = sliders;
        currentInteractableIndex = 0;
        HighlightInteractable(currentInteractables[currentInteractableIndex]);

        AudioMenu.visible = true;
        VideoMenu.visible = false;
        LanguageMenu.visible = false;
    }

    void GameVideo(ClickEvent clk)
    {
        Debug.Log("Video...");
        menuHistory.Push(OptionsMenu); // Save the current menu state
        AudioMenu.visible = false;
        VideoMenu.visible = true;
        LanguageMenu.visible = false;
    }

    void GameControls(ClickEvent clk)
    {
        Debug.Log("Controls...");
        menuHistory.Push(OptionsMenu); // Save the current menu state
        AudioMenu.visible = false;
        VideoMenu.visible = false;
        LanguageMenu.visible = false;
    }

    void GameLanguage(ClickEvent clk)
    {
        Debug.Log("Language...");
        menuHistory.Push(OptionsMenu); // Save the current menu state
        AudioMenu.visible = false;
        VideoMenu.visible = false;
        LanguageMenu.visible = true;
    }

    void GamePLay(ClickEvent clk)
    {
        Debug.Log("Gameplay...");
        AudioMenu.visible = false;
        VideoMenu.visible = false;
        LanguageMenu.visible = false;
    }

    void RestorePreviousMenu()
    {
        if (menuHistory.Count > 0)
        {
            // Hide all menus
            AudioMenu.visible = false;
            VideoMenu.visible = false;
            LanguageMenu.visible = false;
            OptionsMenu.visible = false;

            // Restore the previous menu
            VisualElement lastMenu = menuHistory.Pop();
            lastMenu.visible = true;

            // Update current interactables based on the previous menu
            if (lastMenu == BaseMenu)
            {
                currentInteractables = baseInteractables;
            }
            else if (lastMenu == OptionsMenu)
            {
                currentInteractables = optionInteractables;
            }

            // Reset the current interactable index to the first element
            currentInteractableIndex = 0;
            HighlightInteractable(currentInteractables[currentInteractableIndex]);
        }
    }
}

