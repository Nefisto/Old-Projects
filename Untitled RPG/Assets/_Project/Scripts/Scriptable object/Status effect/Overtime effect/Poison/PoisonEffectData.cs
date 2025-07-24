using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public interface IStackableStatusEffect
{
    public Action OnUpdatedStackAmount { get; set; }
    public int StackAmount { get; set; }
}

[CreateAssetMenu(fileName = "Poison", menuName = EditorConstants.MenuAssets.DOT_EFFECT + "Poison", order = 0)]
public partial class PoisonEffectData : OverTimeEffectData, IStackableStatusEffect
{
    // Placeholder property on material to simulate poison
    private static readonly int HologramBlend = Shader.PropertyToID("_HologramBlend");

    public override StatusEffectKind Kind => StatusEffectKind.Poison;
    public override string MessageOnScreen => "Poison";

    [TitleGroup("Settings")]
    [MinValue(1)]
    [OdinSerialize]
    public int Damage { get; protected set; } = 3;

    public override IEnumerator Setup (StatusEffectContext ctx)
    {
        yield return base.Setup(ctx);

        yield return BeginAnimation(ctx);
        ShowEffectMessageOnScreen(ctx);

        StackAmount = 1;
    }

    protected override void Apply()
    {
        var target = cachedContext.target;
        var damageRoutine = target.TakePoisonDamage(Damage * StackAmount);
        target.StartCoroutine(damageRoutine);
    }

    public override IEnumerator End()
    {
        var material = cachedContext.target.AnimationController.SpriteRenderer.material;
        material.SetFloat(HologramBlend, 0f);

        yield return base.End();
    }

    public override IEnumerator Renew (StatusEffectContext ctx)
    {
        timer = Duration;
        StackAmount = Mathf.Min(StackAmount + 1, Database.GameConstantsSo.MaxAmountOfPoisonStacks);
        OnUpdatedStackAmount?.Invoke();
        yield break;
    }

    private static object BeginAnimation (StatusEffectContext ctx)
        =>
            // var material = ctx.target.AnimationController.UiImage.material;
            // var tween = DOTween.To(
            //     () => material.GetFloat(HologramBlend),
            //     value => material.SetFloat(HologramBlend, value),
            //     1f,
            //     1f);
            null; //tween.WaitForCompletion();
}