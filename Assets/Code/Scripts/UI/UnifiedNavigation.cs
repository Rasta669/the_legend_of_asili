using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;

public class UnifiedNavigation : MonoBehaviour
{
    public RectTransform contentPanel; // The parent container of buttons
    public ScrollRect scrollRect;      // ScrollRect for scrolling
    public float scrollSpeed = 10f;

    private PlayerControls playerControls; // New Input System actions
    private RectTransform[] options;      // Array of all buttons
    private int currentIndex = 0;         // Current keyboard-selected index
    private bool usingMouse = false;      // Whether the mouse is currently being used

    private void Awake()
    {
        // Initialize Player Controls
        playerControls = new PlayerControls();

        // Enable navigation and mouse point actions
        playerControls.UI.Navigate.Enable();
        playerControls.UI.Point.Enable();
        playerControls.UI.Submit.Enable();

        // Attach input listeners
        playerControls.UI.Navigate.performed += OnNavigatePerformed;
        playerControls.UI.Point.performed += OnMouseMoved;
    }

    private void Start()
    {
        if (contentPanel == null || scrollRect == null)
        {
            Debug.LogError("Please assign the Content Panel and ScrollRect in the Inspector.");
            return;
        }

        // Gather all buttons from the content panel
        options = new RectTransform[contentPanel.childCount];
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            options[i] = contentPanel.GetChild(i).GetComponent<RectTransform>();
        }

        // Highlight the first button
        HighlightOption(currentIndex);
    }

    private void OnDestroy()
    {
        // Unsubscribe from input listeners
        playerControls.UI.Navigate.performed -= OnNavigatePerformed;
        playerControls.UI.Point.performed -= OnMouseMoved;
    }

    private void OnNavigatePerformed(InputAction.CallbackContext context)
    {
        if (usingMouse) return; // Ignore keyboard input if the mouse is active

        Vector2 input = context.ReadValue<Vector2>();

        if (input.y > 0.1f) // Move up
        {
            Navigate(-1);
        }
        else if (input.y < -0.1f) // Move down
        {
            Navigate(1);
        }
    }

    private void OnMouseMoved(InputAction.CallbackContext context)
    {
        usingMouse = true; // Switch to mouse input mode

        // Reset the EventSystem selection if hovering with the mouse
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Navigate(int direction)
    {
        usingMouse = false; // Switch to keyboard input mode

        int newIndex = Mathf.Clamp(currentIndex + direction, 0, options.Length - 1);
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            HighlightOption(currentIndex);
            ScrollToOption(currentIndex);
        }
    }

    private void HighlightOption(int index)
    {
        for (int i = 0; i < options.Length; i++)
        {
            var button = options[i].GetComponent<Button>();
            if (button != null)
            {
                if (i == index)
                {
                    // Highlight this button
                    button.Select();
                }
            }
        }
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

    // Optional: Handle mouse hover via Unity's built-in event system
    public void OnButtonHover(GameObject hoveredButton)
    {
        usingMouse = true; // Switch to mouse input mode
        EventSystem.current.SetSelectedGameObject(hoveredButton);
    }
}
