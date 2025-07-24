using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public interface IATBModifier
{
    public float Modifier { get; set; }
}

[CreateAssetMenu(fileName = "Slow", menuName = EditorConstants.MenuAssets.ATTRIBUTE_MODIFIER + "Slow", order = 0)]
public class Slow : StatusEffectData, IATBModifier
{
    public override StatusEffectKind Kind => StatusEffectKind.Slow;

    public override string MessageOnScreen => "Slow";

    [field: TitleGroup("Settings")]
    [field: Range(-1f, 1f)]
    [field: SerializeField]
    public float Modifier { get; set; }

    public override IEnumerator Setup (StatusEffectContext ctx)
    {
        yield return base.Setup(ctx);

        ctx.target.actorATBIcon.EnableSubIcon(Kind);
    }

    public override IEnumerator End()
    {
        yield return base.End();
        
        cachedContext.target.actorATBIcon.DisableSubIcon(Kind);
    }
}