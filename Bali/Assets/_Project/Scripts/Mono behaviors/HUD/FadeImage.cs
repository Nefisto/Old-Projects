using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField]
    private Image image;

    public void SetColor (Color newColor)
        => image.color = newColor;

    public IEnumerator FadeOut (float timeToFinish)
    {
        var originalColor = image.color;
        var targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        var timer = 0f;
        while (timer <= timeToFinish)
        {
            var completedPercent = timer / timeToFinish;
            var newColor = Color.Lerp(originalColor, targetColor, completedPercent);
            image.color = newColor;
            timer += Time.deltaTime;
            yield return null;
        }
        image.color = targetColor;
    }
}