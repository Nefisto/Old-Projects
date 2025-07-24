using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Coin throw increase",
    menuName = EditorConstants.MenuAssets.INSTANT_CHARGE_SKILLS + "Coin throw increase", order = 0)]
public class CoinThrowIncreaseInstant : InstantChargeAbility
{
    public override StatusEffectKind IconKind => StatusEffectKind.CoinThrow;

    public override IEnumerator ApplyAbility (BattleActionContext context)
    {
        var abilityMod = Database
            .GameConstantsSo
            .GetChargeAbilityMod(this);

        var usedMoney = Mathf.RoundToInt(context.caster.Currency * abilityMod.multiplier);
        context.caster.Currency -= usedMoney;

        foreach (var actionInfo in context)
        {
            var bonusMultiplierDamage =
                usedMoney / Database.GameConstantsSo.AmountOfCoinsPerOnePercentIncreaseInCoinThrow;
            actionInfo.percentageOfFlatDamageBonus.Add(("Coin thrown", bonusMultiplierDamage / 100f));
        }

        yield break;
    }
}