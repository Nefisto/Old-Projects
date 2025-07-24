using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin taskmaster", menuName = EditorConstants.GOBLIN_PATH + "Goblin taskmaster")]
public class GoblinTaskmaster : UnitData
{
    public override UnitSide UnitSide => UnitSide.Goblin;
    public override UnitKind UnitKind => UnitKind.GoblinTaskmaster;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        TurnContext.HasSuccessfullyPerformedAction = true;
        CommonOperations.CancelNotificationOnAllGrid();

        var possibleTargets = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(bd => bd.HasDwarfUnit)
            .ToList();

        if (possibleTargets.IsEmpty())
            yield break;

        yield return possibleTargets
            .Shuffle()
            .First()
            .ApplyDamage(new BlockData.ApplyDamageSettings() { damage = Damage });

        yield return CommonOperations.ProcessDeathAnimation();
    }
}