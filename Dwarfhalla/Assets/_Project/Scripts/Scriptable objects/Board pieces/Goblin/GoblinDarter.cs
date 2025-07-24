using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin darter", menuName = EditorConstants.GOBLIN_PATH + "Goblin darter")]
public class GoblinDarter : UnitData
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Poison poisonToApply;

    public override UnitSide UnitSide => UnitSide.Goblin;
    public override UnitKind UnitKind => UnitKind.GoblinDarter;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetPlusAndCrossPatterFrom;

    public override ForeseeAction GetForeseeActions (Vector2Int foreseePosition)
    {
        var foreseeActions = base.GetForeseeActions(foreseePosition);

        // Instead of recalculate all possible targets that I can hit and the poison, we just iterate over the guyz
        //  that can be damaged and create a poison entry to them
        foreach (var (actionType, actionResult) in foreseeActions.actionTypeToTargets.ToNDictionary(pair => pair.Key,
                     pair => pair.Value))
        {
            if (actionType != ActionType.SingleDamage)
                continue;

            var a = actionResult
                .Cast<DamageActionResult>()
                .Where(rb => rb.willTakeDamage)
                .Select(r => new PoisonActionResult() { target = r.target })
                .Cast<ForeseeActionResult>()
                .ToList();

            if (!foreseeActions.actionTypeToTargets.TryAdd(ActionType.Poison, a))
                foreseeActions.actionTypeToTargets[ActionType.Poison] = a;
        }

        return foreseeActions;
    }

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var possibleTargets = CommonOperations
            .GetCrossAndPlusDictionaryDivideDictionaryByDirectionsFrom(PiecePosition, 5)
            .Select(tuple => tuple.Value.FirstOrDefault(bd => bd.HasUnitOnIt))
            .Where(bd => bd is { HasDwarfUnit: true })
            .ToList();

        if (possibleTargets.IsEmpty())
            yield break;

        var selectedTarget = possibleTargets
            .Shuffle()
            .First();

        var targetData = selectedTarget.UnitData;
        yield return selectedTarget.ApplyDamage(new BlockData.ApplyDamageSettings { damage = Damage });

        if (targetData.IsDead)
            yield break;

        yield return selectedTarget.ApplyModifier(new ModifierSettings
        {
            caster = this,
            target = selectedTarget.UnitData,
            modifier = poisonToApply.GetInstance
        });
    }
}