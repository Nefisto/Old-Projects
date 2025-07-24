using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = EditorConstants.MenuAssets.DEBILITATOR_EFFECT + "Stun", order = 0)]
public class StunEffect : DebilitatorEffectData
{
    public override StatusEffectKind Kind => StatusEffectKind.Stun;
    public override string MessageOnScreen => "Stun";

    public override IEnumerator Setup (StatusEffectContext ctx)
    {
        yield return base.Setup(ctx);

        var floatTextSettings = new FloatTextSettings(MessageOnScreen, ctx.target.transform, textColor: Color.yellow);
        ServiceLocator.FloatText.AddCustomFloatText(floatTextSettings, 0);
    }
}