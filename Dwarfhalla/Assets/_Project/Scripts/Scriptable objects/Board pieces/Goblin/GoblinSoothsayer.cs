using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin soothsayer", menuName = EditorConstants.GOBLIN_PATH + "Goblin soothsayer")]
public class GoblinSoothsayer : UnitData
{
    public override UnitSide UnitSide => UnitSide.Goblin;
    public override UnitKind UnitKind => UnitKind.GoblinSoothsayer;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    public override ForeseeAction GetForeseeActions (Vector2Int foreseePosition)
    {
        var foreseeActions = base.GetForeseeActions(foreseePosition);

        var healingBlocks = GetDamagePattern(new PatternGetSettings { position = foreseePosition })
            .GetBlocksFromPositions()
            .Where(bd => bd.HasGoblinUnit)
            .ToList();
        
        if (healingBlocks.IsEmpty())
            return foreseeActions;

        var healingActionResults = healingBlocks
            .Select(b => new HealingActionResult() { target = b.UnitData })
            .Cast<ForeseeActionResult>()
            .ToList();
        
        foreseeActions.actionTypeToTargets.Add(ActionType.SingleHeal, healingActionResults);
        
        return foreseeActions;
    }

    public override IEnumerator PreviewAttack (PreviewSettings settings = null)
    {
        var correctPosition = settings?.CustomPosition ?? PiecePosition;

        var damageBlocks = GetDamagePattern(new PatternGetSettings { position = correctPosition })
            .GetBlocksFromPositions()
            .ToList();
        var healingBlocks = GetDamagePattern(new PatternGetSettings { position = correctPosition })
            .GetBlocksFromPositions()
            .ToList();

        while (true)
        {
            damageBlocks.ForEach(b => b.RuntimeBlock.Notify(RuntimeBlock.NotificationType.Damage));

            yield return new WaitForSeconds(1f);

            damageBlocks.ForEach(b => b.RuntimeBlock.CancelNotification());
            healingBlocks.ForEach(b => b.RuntimeBlock.Notify(RuntimeBlock.NotificationType.Healer));

            yield return new WaitForSeconds(1f);

            healingBlocks.ForEach(b => b.RuntimeBlock.CancelNotification());
        }
    }

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        TurnContext.HasSuccessfullyPerformedAction = true;
        CommonOperations.CancelNotificationOnAllGrid();

        var damageBlock = GetDamagePattern(new PatternGetSettings { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(b => b.HasDwarfUnit)
            .Shuffle()
            .FirstOrDefault();

        var healingBlock = GetDamagePattern(new PatternGetSettings { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(b => b.HasGoblinUnit)
            .Shuffle()
            .FirstOrDefault();

        yield return healingBlock?.ApplyHealing(new BlockData.ApplyHealingSettings { healing = 1 });
        yield return damageBlock?.ApplyDamage(new BlockData.ApplyDamageSettings { damage = Damage });
    }
}