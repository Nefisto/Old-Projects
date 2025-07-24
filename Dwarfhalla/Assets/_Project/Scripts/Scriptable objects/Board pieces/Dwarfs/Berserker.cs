using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Berserker", menuName = EditorConstants.DWARF_PATH + "Berserker")]
public class Berserker : UnitData
{
    public override UnitSide UnitSide => UnitSide.Dwarf;
    public override UnitKind UnitKind => UnitKind.Berserker;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var blocksWithEnemies = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(bd => bd.HasGoblinUnit)
            .Shuffle()
            .ToList();

        if (blocksWithEnemies.IsEmpty())
            yield break;

        foreach (var targetBlock in blocksWithEnemies)
            yield return targetBlock.ApplyDamage(new BlockData.ApplyDamageSettings() { damage = Damage });
    }
}