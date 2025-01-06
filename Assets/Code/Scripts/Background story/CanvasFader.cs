using System.Collections;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    public CanvasGroup canvas1; // First Canvas Group
    public CanvasGroup canvas2; // Second Canvas Group
    public float fadeDuration = 0.5f;

    public void TransitionToCanvas2()
    {
        StartCoroutine(FadeCanvas(canvas1, 1, 0)); // Fade out Canvas 1
        StartCoroutine(FadeCanvas(canvas2, 0, 1)); // Fade in Canvas 2
    }

    public void TransitionToCanvas1()
    {
        StartCoroutine(FadeCanvas(canvas2, 1, 0)); // Fade out Canvas 2
        StartCoroutine(FadeCanvas(canvas1, 0, 1)); // Fade in Canvas 1
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        canvasGroup.interactable = endAlpha == 1;
        canvasGroup.blocksRaycasts = endAlpha == 1;
    }
}
