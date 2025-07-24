using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing", menuName = EditorConstants.MenuAssets.ACTIVE_SKILLS + "Healing", order = 0)]
public class Healing : LevelChargeSkill
{
    [TabGroup("Tab", "Common")]
    [TitleGroup("Tab/Common/Specific")]
    [SerializeField]
    private int healingAmount;

    [TitleGroup("Tab/Common/Specific")]
    [SerializeField]
    private StatusEffectData regeneration;

    protected override IEnumerator BeforeCalculateActionValues (BattleActionContext context)
    {
        yield return base.BeforeCalculateActionValues(context);

        foreach (var actionInfo in context)
            actionInfo.flatHeal = healingAmount;

        if (GetChargeLevel <= 1)
            yield break;

        foreach (var actionInfo in context)
            actionInfo.percentageOfFlatHealBonus.Add(($"{nameof(Healing)} charge", .5f));
    }

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        foreach (var actionInfo in context)
            yield return actionInfo.target.HealHealth(actionInfo);

        HoldFinish(new HoldFinishSettings { shouldHideBar = true });
    }

    protected override IEnumerator AfterCalculateActionValues (ActionInfo info)
    {
        yield return base.AfterCalculateActionValues(info);

        if (GetChargeLevel < 3)
            yield break;

        info.effectInfo.Add(new EffectInfo
        {
            data = regeneration,
            baseChanceToApplyEffect = 1f
        });
    }

    protected override int GetDamage (ActorData data) => 0;
}