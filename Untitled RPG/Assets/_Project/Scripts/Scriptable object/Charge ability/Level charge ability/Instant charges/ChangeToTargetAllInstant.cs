using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Change target to all",
    menuName = EditorConstants.MenuAssets.INSTANT_CHARGE_SKILLS + "Change target to all", order = 0)]
public class ChangeToTargetAllInstant : InstantChargeAbility
{
    public override StatusEffectKind IconKind => StatusEffectKind.TargetAll;

    public override IEnumerator ApplyAbility (BattleActionContext context)
    {
        var newTargets = ServiceLocator
            .TargetController
            .GetTargets(GroupTarget.AllEnemies, context.caster)
            .ToList();

        context.ChangeTargets(newTargets);
        yield break;
    }
}