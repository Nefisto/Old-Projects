using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Temporary part for visual feedback when something happen to a character
/// </summary>
public abstract partial class BattleActor
{
    [Title("Temporary")]
    public Color to;
    public float blinkDuration = .5f;

    private Color originalColor;
    
    [DisableInEditorMode]
    [Button]
    protected IEnumerator BlinkFeedback()
    {
        spriteRenderer.DOKill();
        spriteRenderer.color = originalColor;

        var blinkTween = spriteRenderer
            .DOColor(to, blinkDuration)
            .OnComplete(() => spriteRenderer.color = originalColor);

        yield return blinkTween.WaitForCompletion();
    }
}