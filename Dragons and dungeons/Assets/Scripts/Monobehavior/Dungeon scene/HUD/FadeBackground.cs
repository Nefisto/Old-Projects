using System.Collections;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FadeBackground : LazyBehavior
{
    [Title("Settings")]
    [SerializeField]
    private float fadeOutDuration = 2f;

    [SerializeField]
    private Ease fadeOutEase = Ease.Linear;

    [SerializeField]
    private float fadeInDuration = 2f;

    [SerializeField]
    private Ease fadeInEase = Ease.Linear;

    [Space]
    [SerializeField]
    private Image background;

    private void Start()
    {
        var color = image.color;
        color = new Color(color.r, color.g, color.b, 1f);
        image.color = color;
    }

    public IEnumerator FadeOutRoutine()
    {
        background.DOComplete();
        
        SetBackgroundAlpha(1f);

        var fadeTween = background
            .DOFade(0f, fadeOutDuration)
            .SetEase(fadeOutEase);

        yield return fadeTween.WaitForCompletion();
        
        gameObject.SetActive(false);
    }

    public IEnumerator FadeInRoutine()
    {
        background.DOComplete();
        
        gameObject.SetActive(true);
        
        SetBackgroundAlpha(0f);

        var fadeTween = background
            .DOFade(1f, fadeInDuration)
            .SetEase(fadeInEase);

        yield return fadeTween.WaitForCompletion();
    }

    public void SetBackgroundAlpha (float alpha)
    {
        gameObject.SetActive(true);
        
        var color = background.color;
        background.color = new Color(color.r, color.g, color.b, alpha);
    }
    
#if UNITY_EDITOR
    [DisableInEditorMode]
    [Button]
    private void FadeOut()
        => StartCoroutine(FadeOutRoutine());

    [DisableInEditorMode]
    [Button]
    private void FadeIn()
        => StartCoroutine(FadeInRoutine());
#endif
}