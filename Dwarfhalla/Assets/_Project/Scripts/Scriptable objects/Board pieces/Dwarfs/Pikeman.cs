using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Pikeman", menuName = EditorConstants.DWARF_PATH + "Pikeman")]
public class Pikeman : UnitData
{
    public override UnitSide UnitSide => UnitSide.Dwarf;
    public override UnitKind UnitKind => UnitKind.Pikeman;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    public override IEnumerator PreviewAttack (PreviewSettings settings = null)
    {
        var correctInitialPosition = settings?.CustomPosition ?? PiecePosition;
        var blocksThatICanDamage =
            GetDamagePattern(new PatternGetSettings { position = correctInitialPosition })
                .GetBlocksFromPositions();
        foreach (var possibleBlock in blocksThatICanDamage)
        {
            possibleBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.Damage);

            var direction = correctInitialPosition.GetDirectionTo(possibleBlock.Position);
            var behindBlock = CommonOperations
                .GetBlockDataAt(possibleBlock.Position + direction);

            behindBlock?.RuntimeBlock.Notify(RuntimeBlock.NotificationType.PossibleBlock);
        }

        yield break;
    }

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        TurnContext.HasSuccessfullyPerformedAction = true;
        CommonOperations.CancelNotificationOnAllGrid();

        var blocksWithEnemy = GetDamagePattern(new PatternGetSettings { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(bd => bd.HasGoblinUnit)
            .ToList();

        if (blocksWithEnemy.IsEmpty())
            yield break;

        var randomizedBlock = blocksWithEnemy
            .Shuffle()
            .First();

        yield return randomizedBlock.ApplyDamage(new BlockData.ApplyDamageSettings { damage = Damage });

        var attackDirection = BlockData.Position.GetDirectionTo(randomizedBlock.Position);
        var behindBlock = CommonOperations
            .GetBlockDataAt(randomizedBlock.Position + attackDirection);

        if (behindBlock == null)
            yield break;

        if (!behindBlock.HasGoblinUnit)
            yield break;

        yield return behindBlock.ApplyDamage(
            new BlockData.ApplyDamageSettings { damage = Mathf.RoundToInt(Damage * .5f) });
    }
}