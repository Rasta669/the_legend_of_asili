using UnityEngine;
using TMPro;
using System.Collections;

public class HintTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText; // Reference to the TMP text component
    [SerializeField] private CanvasGroup canvasGroup; // Reference to the CanvasGroup for fading

    [SerializeField] private float fadeDuration = 0.5f; // Time for fading in/out

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = hintText.GetComponent<CanvasGroup>();
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; // Ensure the text starts invisible
        }
    }

    public void ShowHint(string message)
    {
        hintText.text = message; // Update the hint text
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    public void HideHint()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1; // Ensure fully visible
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0; // Ensure fully invisible
    }
}
