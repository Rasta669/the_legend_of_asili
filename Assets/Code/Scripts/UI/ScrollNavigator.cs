//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class ScrollNavigator : MonoBehaviour
//{
//    public RectTransform contentPanel; // The parent container of the options
//    public ScrollRect scrollRect; // The ScrollRect component
//    public float scrollSpeed = 10f; // Speed of scrolling when navigating
//    private int currentIndex = 0; // The currently selected index
//    private RectTransform[] options; // Array of option RectTransforms
//    private Vector2 velocity = Vector2.zero;

//    private void Start()
//    {
//        if (contentPanel == null || scrollRect == null)
//        {
//            Debug.LogError("Please assign the Content Panel and ScrollRect in the Inspector.");
//            return;
//        }

//        // Cache all the children of the content panel as options
//        options = new RectTransform[contentPanel.childCount];
//        for (int i = 0; i < contentPanel.childCount; i++)
//        {
//            options[i] = contentPanel.GetChild(i).GetComponent<RectTransform>();
//        }

//        HighlightOption(currentIndex); // Highlight the first option
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.W)) // Navigate up
//        {
//            Navigate(-1);
//        }
//        else if (Input.GetKeyDown(KeyCode.S)) // Navigate down
//        {
//            Navigate(1);
//        }
//    }

//    private void Navigate(int direction)
//    {
//        // Calculate the new index
//        int newIndex = Mathf.Clamp(currentIndex + direction, 0, options.Length - 1);

//        if (newIndex != currentIndex)
//        {
//            currentIndex = newIndex;
//            HighlightOption(currentIndex);
//            ScrollToOption(currentIndex);
//        }
//    }

//    private void HighlightOption(int index)
//    {
//        // Example of highlighting: Set the selected option's color
//        for (int i = 0; i < options.Length; i++)
//        {
//            var button = options[i].GetComponent<Button>();
//            if (button != null)
//            {
//                ColorBlock colors = button.colors;
//                colors.normalColor = (i == index) ? Color.yellow : Color.white;
//                button.colors = colors;
//            }
//        }

//        // Optionally, set focus for keyboard/gamepad navigation
//        EventSystem.current.SetSelectedGameObject(options[index].gameObject);
//    }

//    //private void ScrollToOption(int index)
//    //{
//    //    // Calculate the normalized position of the selected option
//    //    float contentHeight = contentPanel.rect.height;
//    //    float viewportHeight = scrollRect.viewport.rect.height;

//    //    // Position of the option relative to the content panel
//    //    float targetY = -options[index].anchoredPosition.y;

//    //    // Adjust the content panel's position to bring the option into view
//    //    float clampedY = Mathf.Clamp(targetY, -(contentHeight - viewportHeight), 0);
//    //    Vector2 targetPosition = new Vector2(contentPanel.anchoredPosition.x, clampedY);

//    //    // Smoothly move the content panel to the target position
//    //    contentPanel.anchoredPosition = Vector2.Lerp(contentPanel.anchoredPosition, targetPosition, Time.deltaTime * scrollSpeed);
//    //}


//    private void ScrollToOption(int index)
//    {
//        // Calculate the normalized position of the selected option
//        float contentHeight = contentPanel.rect.height;
//        float viewportHeight = scrollRect.viewport.rect.height;

//        // Position of the option relative to the content panel
//        float targetY = -options[index].anchoredPosition.y;

//        // Adjust the content panel's position to bring the option into view
//        float clampedY = Mathf.Clamp(targetY, -(contentHeight - viewportHeight), 0);
//        Vector2 targetPosition = new Vector2(contentPanel.anchoredPosition.x, clampedY);

//        // Smoothly move the content panel to the target position
//        contentPanel.anchoredPosition = Vector2.SmoothDamp(
//            contentPanel.anchoredPosition,
//            targetPosition,
//            ref velocity,
//            0.1f // Smooth time
//        );
//    }

//}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollNavigator : MonoBehaviour
{
    public RectTransform contentPanel;
    public ScrollRect scrollRect;
    public float scrollSpeed = 10f;
    private int currentIndex = 0;
    private RectTransform[] options;

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            Navigate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Navigate(1);
        }
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

    private void HighlightOption(int index)
    {
        for (int i = 0; i < options.Length; i++)
        {
            var button = options[i].GetComponent<Button>();
            if (button != null)
            {
                ColorBlock colors = button.colors;
                colors.normalColor = (i == index) ? Color.yellow : Color.white;
                button.colors = colors;
            }
        }

        EventSystem.current.SetSelectedGameObject(options[index].gameObject);
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

