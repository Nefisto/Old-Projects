using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Javeliner", menuName = EditorConstants.DWARF_PATH + "Javeliner")]
public class Javeliner : UnitData
{
    public override UnitSide UnitSide => UnitSide.Dwarf;
    public override UnitKind UnitKind => UnitKind.Javeliner;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetCrossPatternFrom;

    public override IEnumerator PreviewAttack (PreviewSettings settings = null)
    {
        var position = settings?.CustomPosition ?? PiecePosition;

        foreach (var (_, blocks) in CommonOperations.GetCrossPatternDividedByDirectionsFrom(position, 5))
        {
            var disabledLine = false;
            var markNextBlockAsPossible = false;
            foreach (var blockData in blocks)
            {
                if (markNextBlockAsPossible)
                {
                    blockData.RuntimeBlock.Notify(RuntimeBlock.NotificationType.PossibleBlock);
                    markNextBlockAsPossible = false;
                    continue;
                }

                if (disabledLine)
                {
                    blockData.RuntimeBlock.Notify(RuntimeBlock.NotificationType.ImpossibleBlock);
                    continue;
                }

                if (blockData.HasUnitOnIt)
                {
                    disabledLine = true;
                    markNextBlockAsPossible = true;
                }

                blockData.RuntimeBlock.Notify(RuntimeBlock.NotificationType.Damage);
            }
        }

        yield break;
    }

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var initialBlock = BlockData;
        var possibleTargets = CommonOperations
            .GetCrossPatternDividedByDirectionsFrom(PiecePosition.x, PiecePosition.y, 5)
            .Select(tuple => tuple.Value.FirstOrDefault(bd => bd.HasUnitOnIt))
            .Where(bd => bd is { HasGoblinUnit: true })
            .ToList();

        if (possibleTargets.IsEmpty())
            yield break;

        var selectedBlock = possibleTargets
            .Shuffle()
            .First();
        var targetUnit = selectedBlock.UnitData;
        yield return selectedBlock.ApplyDamage(new BlockData.ApplyDamageSettings { damage = Damage });

        if (targetUnit.IsDead)
            yield break;

        var direction = initialBlock.Position.GetDirectionTo(selectedBlock.Position);
        var blockBehindTarget = CommonOperations
            .GetBlockDataAt(selectedBlock.Position + direction);

        if (blockBehindTarget == null)
            yield break;

        yield return new Movement
        {
            InitialBlock = selectedBlock,
            FinalBlock = blockBehindTarget
        }.Run();
    }
}