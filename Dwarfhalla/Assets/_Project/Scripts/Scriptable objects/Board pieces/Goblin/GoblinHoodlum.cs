using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin hoodlum", menuName = EditorConstants.GOBLIN_PATH + "Goblin hoodlum")]
public class GoblinHoodlum : UnitData
{
    public override UnitSide UnitSide => UnitSide.Goblin;
    public override UnitKind UnitKind => UnitKind.GoblinCaveHoodlum;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations
            .GetHorizontallyPatternDividedByDirectionsFrom;

    public override IEnumerator PreviewAttack (PreviewSettings settings = null)
    {
        var position = settings?.CustomPosition ?? PiecePosition;

        var directionToBlocks = CommonOperations
            .GetHorizontallyPatternDividedByDirectionsFrom(position.x, position.y, 5);

        foreach (var (_, blocks) in directionToBlocks)
        {
            var disableLine = false;
            foreach (var block in blocks)
            {
                if (disableLine)
                {
                    block.RuntimeBlock.Notify(RuntimeBlock.NotificationType.ImpossibleBlock);
                    continue;
                }

                if (block.HasUnitOnIt)
                {
                    disableLine = true;
                    block.RuntimeBlock.Notify(block.HasDwarfUnit
                        ? RuntimeBlock.NotificationType.Damage
                        : RuntimeBlock.NotificationType.ImpossibleBlock);
                    continue;
                }

                block.RuntimeBlock.Notify(RuntimeBlock.NotificationType.Damage);
            }
        }

        yield break;
    }

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        TurnContext.HasSuccessfullyPerformedAction = true;
        CommonOperations.CancelNotificationOnAllGrid();

        var possibleTargets = CommonOperations
            .GetHorizontallyPatternDividedByDirectionsFrom(PiecePosition.x, PiecePosition.y, 5)
            .Select(dict => dict.Value.FirstOrDefault(b => b.HasDwarfUnit))
            .Where(bd => bd != null)
            .ToList();

        if (possibleTargets.IsEmpty())
            yield break;

        foreach (var target in possibleTargets)
            yield return target.ApplyDamage(new BlockData.ApplyDamageSettings { damage = Damage });
    }
}