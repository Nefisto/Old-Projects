using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static IScreenFading;

// TODO: Move it to NTools
public partial class ScreenFading : MonoBehaviour, IScreenFading
{
    [TitleGroup("References")]
    [SerializeField]
    private Image image;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool isFaded;

    private void Awake() => ServiceLocator.ScreenFading = this;

    public IEnumerator FadeOut (Settings settings = null)
    {
        if (isFaded)
            yield break;

        yield return Fade(settings ?? new Settings(), 1f, 0f);
        image.raycastTarget = false;
        isFaded = true;
    }

    public IEnumerator FadeIn (Settings settings = null)
    {
        if (!isFaded)
            yield break;

        image.raycastTarget = true;
        yield return Fade(settings ?? new Settings(), 0f, 1f);
        isFaded = false;
    }

    private IEnumerator Fade (Settings settings, float from, float to)
    {
        image.color = settings.color;
        // TODO: Create SetAlpha on NTools
        image.color = Helper.ColorHelper.SetAlpha(image.color, from);

        var timer = 0f;
        while (timer < settings.duration)
        {
            var lastFrameDuration = Time.deltaTime;

            var currentPercentage = Mathf.Clamp(timer / settings.duration, 0f, 1f);
            var alphaPercentage = from > to ? 1 - currentPercentage : currentPercentage;

            image.color = Helper.ColorHelper.SetAlpha(image.color, alphaPercentage);

            timer += lastFrameDuration;
            yield return null;
        }

        image.color = Helper.ColorHelper.SetAlpha(image.color, to);
    }
}