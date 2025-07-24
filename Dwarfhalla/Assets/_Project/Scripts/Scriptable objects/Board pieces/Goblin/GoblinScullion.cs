using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin scullion", menuName = EditorConstants.GOBLIN_PATH + "Goblin scullion")]
public class GoblinScullion : UnitData
{
    public override UnitSide UnitSide => UnitSide.Goblin;
    public override UnitKind UnitKind => UnitKind.GoblinScout;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var randomizedBlock = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(bd => bd.HasDwarfUnit)
            .Shuffle()
            .FirstOrDefault();

        if (randomizedBlock == null)
            yield break;

        var hasTaskmasterNear = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .Any(bd => bd.HasGoblinUnit && bd.UnitData.UnitKind == UnitKind.GoblinTaskmaster);

        yield return randomizedBlock.ApplyDamage(
            new BlockData.ApplyDamageSettings() { damage = Damage * (hasTaskmasterNear ? 2 : 1) });
    }
}