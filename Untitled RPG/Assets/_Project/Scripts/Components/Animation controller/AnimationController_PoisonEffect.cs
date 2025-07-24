using System.Collections;
using DG.Tweening;
using UnityEngine;

public partial class AnimationController
{
    private static readonly int HologramBlend = Shader.PropertyToID("_HologramBlend");

    public IEnumerator StartPoisonedAnimation()
    {
        SpriteRenderer.material.SetFloat(HologramBlend, 1f);

        var material = SpriteRenderer.material;
        var tween = DOTween.To(
            () => material.GetFloat(HologramBlend),
            value => material.SetFloat(HologramBlend, value),
            1f,
            1f);

        yield return tween.WaitForCompletion();
    }

    public IEnumerator EndPoisonedAnimation()
    {
        SpriteRenderer.material.SetFloat(HologramBlend, 0f);
        yield return null;
    }
}