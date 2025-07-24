using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Toxic blast",
    menuName = EditorConstants.MenuAssets.ACTIVE_SKILLS + "Toxic blast", order = 0)]
public class ToxicBlast : Skill
{
    public override IEnumerator Setup()
    {
        yield return base.Setup();

        OnFinishedAnimationEntryPoint += FinishedAnimationHandle;
    }

    protected override IEnumerator AfterCalculateActionValues (ActionInfo info)
    {
        yield return base.AfterCalculateActionValues(info);

        var effectController = info.target.StatusEffectController;
        info.hasMissed = !effectController.HasCondition(StatusEffectKind.Poison);

        if (info.hasMissed)
            yield break;

        var poisonStacks = effectController.GetAmountOfStacks(StatusEffectKind.Poison);
        info.percentageOfFlatDamageBonus.Add(("Toxic blast", .5f * poisonStacks));
    }

    private IEnumerator FinishedAnimationHandle (BattleActionContext context)
    {
        foreach (var actionInfo in context)
        {
            var effectController = actionInfo.target.StatusEffectController;

            yield return effectController.CancelEffect(StatusEffectKind.Poison);
        }
    }
}