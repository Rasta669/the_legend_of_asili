//Disable/sync mouse selection on keyboard navigation
// start button muisbehaves on navigating back up

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.InputSystem;

public class ScrollNavigator : MonoBehaviour
{
    public RectTransform contentPanel;
    public ScrollRect scrollRect;
    public float scrollSpeed = 10f;
    private int currentIndex = 0;
    private RectTransform[] options;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.UI.Navigate.Enable();
        playerControls.UI.Navigate.performed += OnNavigatePerformed;
    }

    private void Start()
    {
        if (contentPanel == null || scrollRect == null)
        {
            Debug.LogError("Please assign the Content Panel and ScrollRect in the Inspector.");
            return;
        }

        options = new RectTransform[contentPanel.childCount];
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            options[i] = contentPanel.GetChild(i).GetComponent<RectTransform>();
        }

        HighlightOption(currentIndex);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Navigate(-1);
        //}
        //else if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Navigate(1);
        //}
        //playerControls.UI.Navigate.performed += OnNavigatePerformed;
    }

    private void Navigate(int direction)
    {
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, options.Length - 1);

        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            HighlightOption(currentIndex);
            ScrollToOption(currentIndex);
        }
    }

    //private void OnNavigatePerformed(InputAction.CallbackContext context)
    //{    
    //    if (context.performed)
    //    {
    //        int input = context.ReadValue<int>();
    //        Navigate(input);
    //    }
    //}

    private void OnNavigatePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>(); // Read the Vector2 input

        if (input.y > 0.1f) // Check if the input is moving up
        {
            Navigate(-1); // Move up
        }
        else if (input.y < -0.1f) // Check if the input is moving down
        {
            Navigate(1); // Move down
        }
    }


    //private void HighlightOption(int index)
    //{
    //    for (int i = 0; i < options.Length; i++)
    //    {
    //        var button = options[i].GetComponent<Button>();
    //        if (button != null)
    //        {
    //            ColorBlock colors = button.colors;
    //            colors.normalColor = (i == index) ? Color.yellow : Color.white;
    //            button.colors = colors;
    //        }
    //    }

    //    EventSystem.current.SetSelectedGameObject(options[index].gameObject);
    //}

    private void HighlightOption(int index)
    {
        Debug.Log($"Highlighting option at index: {index}");

        for (int i = 0; i < options.Length; i++)
        {
            var button = options[i].GetComponent<Button>();
            if (button != null)
            {
                ColorBlock colors = button.colors;
                colors.normalColor = (i == index) ? colors.highlightedColor : Color.white;
                button.colors = colors;

                Debug.Log($"Option {i} color set to: {(i == index ? "Yellow" : "White")}");
            }
        }

        // Ensure the EventSystem is active and set the selected GameObject
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(options[index].gameObject);
            Canvas.ForceUpdateCanvases(); // Force the UI to refresh
            Debug.Log($"EventSystem selected GameObject: {options[index].gameObject.name}");
        }
        else
        {
            Debug.LogError("No active EventSystem in the scene.");
        }
    }


    private IEnumerator SmoothScroll(Vector2 targetPosition)
    {
        while (Vector2.Distance(contentPanel.anchoredPosition, targetPosition) > 0.1f)
        {
            contentPanel.anchoredPosition = Vector2.Lerp(
                contentPanel.anchoredPosition,
                targetPosition,
                Time.deltaTime * scrollSpeed
            );
            yield return null;
        }
        contentPanel.anchoredPosition = targetPosition;
    }

    private void ScrollToOption(int index)
    {
        float contentHeight = contentPanel.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        float targetY = -options[index].anchoredPosition.y;
        float clampedY = Mathf.Clamp(targetY, -(contentHeight - viewportHeight), 0);
        Vector2 targetPosition = new Vector2(contentPanel.anchoredPosition.x, clampedY);

        StopAllCoroutines();
        StartCoroutine(SmoothScroll(targetPosition));
    }
}

