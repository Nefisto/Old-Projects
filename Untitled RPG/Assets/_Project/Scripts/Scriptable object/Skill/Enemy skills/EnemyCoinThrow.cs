using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Coin throw", menuName = EditorConstants.MenuAssets.ENEMY_SKILLS + "Coin throw",
    order = 0)]
public class EnemyCoinThrow : Skill
{
    [TitleGroup("Settings")]
    [MinMaxSlider(0f, 1f, true)]
    [SerializeField]
    private Vector2 percentageOfMoneyUsed;

    [TitleGroup("Settings")]
    [SerializeField]
    private int minimumThresholdToDontMiss = 200;

    [TitleGroup("Settings")]
    [SerializeField]
    private int amountOfCoinsPerOnePercentIncrease = 10;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int moneyUsedOnLastCast;

    protected override IEnumerator BeforeCalculateActionValues (BattleActionContext context)
    {
        yield return base.BeforeCalculateActionValues(context);

        var percentageOfCurrencyUsed = percentageOfMoneyUsed.GetRandom();
        moneyUsedOnLastCast = Mathf.RoundToInt(context.caster.Currency * percentageOfCurrencyUsed);
        context.caster.Currency -= moneyUsedOnLastCast;
        var bonusMultiplierDamage = moneyUsedOnLastCast / amountOfCoinsPerOnePercentIncrease;
        foreach (var actionInfo in context)
            actionInfo.percentageOfFlatDamageBonus.Add(("Coin thrown", bonusMultiplierDamage / 100f));
    }

    protected override IEnumerator AfterCalculateActionValues (ActionInfo info)
    {
        yield return base.AfterCalculateActionValues(info);

        if (moneyUsedOnLastCast < minimumThresholdToDontMiss)
            yield break;

        info.hasMissed = false;
    }
}