using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Inquisitor", menuName = EditorConstants.DWARF_PATH + "Inquisitor")]
public class Inquisitor : UnitData
{
    public override UnitSide UnitSide => UnitSide.Dwarf;
    public override UnitKind UnitKind => UnitKind.Inquisitor;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var randomizedBlock = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(bd => bd.HasGoblinUnit)
            .Shuffle()
            .FirstOrDefault();

        if (randomizedBlock == null)
            yield break;

        var enemyUnit = randomizedBlock.UnitData;
        yield return randomizedBlock.ApplyDamage(new BlockData.ApplyDamageSettings { damage = Damage });

        if (enemyUnit.IsDead)
            yield return OnKillBehavior();
    }

    public override IEnumerator OnKillBehavior()
    {
        yield return BlockData.ApplyHealing(new BlockData.ApplyHealingSettings { healing = 1 });
    }
}