using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Drain health increase",
    menuName = EditorConstants.MenuAssets.INSTANT_CHARGE_SKILLS + "Drain health increase", order = 0)]
public class DrainHealthInstant : InstantChargeAbility
{
    public override StatusEffectKind IconKind => StatusEffectKind.DrainHealth;

    public override IEnumerator ApplyAbility (BattleActionContext context)
    {
        var drainPercentage = Database
            .GameConstantsSo
            .GetChargeAbilityMod(this)
            .multiplier;

        foreach (var actionInfo in context)
        {
            if (actionInfo.hasMissed)
                continue;

            var healAmount = Mathf.RoundToInt(actionInfo.FinalDamage * drainPercentage);
            healAmount = Mathf.Max(healAmount, 1);
            yield return actionInfo.caster.HealHealth(new ActionInfo { flatHeal = healAmount });
        }
    }
}