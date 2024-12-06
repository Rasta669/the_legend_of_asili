
// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement; // Required for scene loading

// public class CanvasTextFadeController : MonoBehaviour
// {
//     public Canvas[] canvases; // Array of Canvas objects, each containing its own set of text elements
//     public float fadeDuration = 1.0f; // Duration of each fade-in/out animation
//     public float delayBetweenTexts = 0.5f; // Time delay between fading in each text
//     public float waitTimeAfterDisplay = 3.0f; // Time to wait after all texts in the canvas are displayed before fading out
//     public string gameSceneName; // Name of the game scene to load

//     private void Start()
//     {
//         // Initialize all canvases as inactive and set their texts to invisible
//         foreach (Canvas canvas in canvases)
//         {
//             canvas.gameObject.SetActive(false); // Deactivate the canvas initially
//             TextMeshProUGUI[] textElements = canvas.GetComponentsInChildren<TextMeshProUGUI>();
//             foreach (TextMeshProUGUI text in textElements)
//             {
//                 SetTextAlpha(text, 0);
//             }
//         }

//         // Start the canvas display sequence
//         StartCoroutine(ShowCanvasesInSequence());
//     }

//     private IEnumerator ShowCanvasesInSequence()
//     {
//         foreach (Canvas canvas in canvases)
//         {
//             canvas.gameObject.SetActive(true); // Activate the current canvas

//             // Get all text elements within the canvas
//             TextMeshProUGUI[] textElements = canvas.GetComponentsInChildren<TextMeshProUGUI>();

//             // Fade in each text in order
//             foreach (TextMeshProUGUI text in textElements)
//             {
//                 yield return StartCoroutine(FadeText(text, 0, 1)); // Fade in
//                 yield return new WaitForSeconds(delayBetweenTexts); // Wait before the next text
//             }

//             // Wait after all texts are displayed
//             yield return new WaitForSeconds(waitTimeAfterDisplay);

//             // Fade out each text in reverse order
//             for (int i = textElements.Length - 1; i >= 0; i--)
//             {
//                 yield return StartCoroutine(FadeText(textElements[i], 1, 0)); // Fade out
//             }

//             canvas.gameObject.SetActive(false); // Deactivate the canvas after fading out
//         }

//         // Load the game scene after all background story canvases have been displayed
//         Invoke(nameof(BeginPlay), 1.5f);
//     }
//     private void BeginPlay()
//     {
//         SceneManager.LoadScene(gameSceneName);
//     }
//     private IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha)
//     {
//         float elapsedTime = 0.0f;

//         // Get the current color of the text
//         Color color = text.color;

//         while (elapsedTime < fadeDuration)
//         {
//             // Interpolate alpha over time
//             color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
//             text.color = color;

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         // Ensure the final alpha is set
//         color.a = endAlpha;
//         text.color = color;
//     }

//     private void SetTextAlpha(TextMeshProUGUI text, float alpha)
//     {
//         Color color = text.color;
//         color.a = alpha;
//         text.color = color;
//     }

//     public void Skip()
//     {
//         SceneManager.LoadScene(gameSceneName);
//     }
// }


using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Required for scene loading

public class CanvasTextFadeController : MonoBehaviour
{
    public Canvas[] canvases; // Array of Canvas objects, each containing its own set of text elements
    public float fadeDuration = 1.0f; // Duration of each fade-in/out animation
    public float delayBetweenTexts = 0.5f; // Time delay between fading in each text
    public float waitTimeAfterDisplay = 3.0f; // Time to wait after all texts in the canvas are displayed before fading out
    public string gameSceneName; // Name of the game scene to load

    private void Start()
    {
        // Initialize all canvases as inactive and set their texts to invisible
        foreach (Canvas canvas in canvases)
        {
            canvas.gameObject.SetActive(false); // Deactivate the canvas initially
            TextMeshProUGUI[] textElements = canvas.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in textElements)
            {
                SetTextAlpha(text, 0);
            }
        }

        // Start the canvas display sequence
        StartCoroutine(ShowCanvasesInSequence());
    }

    private IEnumerator ShowCanvasesInSequence()
    {
        foreach (Canvas canvas in canvases)
        {
            canvas.gameObject.SetActive(true); // Activate the current canvas

            // Get all text elements within the canvas
            TextMeshProUGUI[] textElements = canvas.GetComponentsInChildren<TextMeshProUGUI>();

            // Fade in the first text element separately
            if (textElements.Length > 0)
            {
                yield return StartCoroutine(FadeText(textElements[0], 0, 1)); // Fade in the first element
                yield return new WaitForSeconds(delayBetweenTexts); // Wait before the next text
            }

            // Fade in the remaining text elements in order
            for (int i = 1; i < textElements.Length; i++)
            {
                yield return StartCoroutine(FadeText(textElements[i], 0, 1)); // Fade in
                yield return new WaitForSeconds(delayBetweenTexts); // Wait before the next text
            }

            // Wait after all texts are displayed
            yield return new WaitForSeconds(waitTimeAfterDisplay);

            // Fade out each text in reverse order
            for (int i = textElements.Length - 1; i >= 0; i--)
            {
                yield return StartCoroutine(FadeText(textElements[i], 1, 0)); // Fade out
            }

            canvas.gameObject.SetActive(false); // Deactivate the canvas after fading out
        }

        // Load the game scene after all background story canvases have been displayed
        Invoke(nameof(BeginPlay), 1.5f);
    }

    private void BeginPlay()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;

        // Get the current color of the text
        Color color = text.color;

        while (elapsedTime < fadeDuration)
        {
            // Interpolate alpha over time
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            text.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is set
        color.a = endAlpha;
        text.color = color;
    }

    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    public void Skip()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
