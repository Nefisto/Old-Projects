using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage increase",
    menuName = EditorConstants.MenuAssets.INSTANT_CHARGE_SKILLS + "Damage increase", order = 0)]
public class DamageIncreaseInstant : InstantChargeAbility
{
    public override StatusEffectKind IconKind => StatusEffectKind.Damage;

    public override IEnumerator ApplyAbility (BattleActionContext context)
    {
        foreach (var actionInfo in context)
        {
            var increasePercentage = Database
                .GameConstantsSo
                .GetChargeAbilityMod(this)
                .multiplier;

            actionInfo.multiplyBaseDamage.Add(new DamageModifierPassiveInfo("[Instant charge] Power increase",
                increasePercentage));
        }

        yield break;
    }
}