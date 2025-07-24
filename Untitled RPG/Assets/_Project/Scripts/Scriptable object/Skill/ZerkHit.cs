using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Zerk hit",
    menuName = EditorConstants.MenuAssets.ACTIVE_SKILLS + "Zerk hit", order = 0)]
public class ZerkHit : LevelChargeSkill
{
    [TitleGroup("Settings")]
    [Tooltip("Percentage increase for every missing health threshold")]
    [SerializeField]
    private float damagePercentageIncrease;

    [TitleGroup("Settings")]
    [Tooltip("Threshold for every damage increase stack")]
    [Range(0f, 1f)]
    [SerializeField]
    private float missingHealthPercentagePerStack;

    protected override IEnumerator BeforeCalculateActionValues (BattleActionContext context)
    {
        var missingHealthPercentage = 1f
                                      - context
                                          .caster
                                          .HealthResource
                                          .CurrentPercentage;

        foreach (var actionInfo in context)
        {
            if (GetChargeLevel > 1)
                missingHealthPercentage += 1f - actionInfo.target.HealthResource.CurrentPercentage;

            var amountOfStack = (int)(missingHealthPercentage / missingHealthPercentagePerStack);
            actionInfo
                .multiplyBaseDamage
                .Add(new DamageModifierPassiveInfo("Missing health stacks from zerk hit",
                    amountOfStack * damagePercentageIncrease));
        }

        yield break;
    }
}