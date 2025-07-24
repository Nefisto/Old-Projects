using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Blind", menuName = EditorConstants.MenuAssets.ATTRIBUTE_MODIFIER + "Blind", order = 0)]
public class Blind : StatusEffectData
{
    [TitleGroup("Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float flatReductionToHitChance = .2f;

    public override StatusEffectKind Kind => StatusEffectKind.Blind;
    public override string MessageOnScreen => "Blind";

    public override IEnumerator ApplyOnCurrentAction (ActionInfo actionInfo)
    {
        actionInfo.flatBonusToHit.Add(("Blind", -flatReductionToHitChance));

        yield return base.ApplyOnCurrentAction(actionInfo);
    }

    public override IEnumerator Renew (StatusEffectContext ctx)
    {
        timer = Duration;
        return base.Renew(ctx);
    }
}