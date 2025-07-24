using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Axeman", menuName = EditorConstants.DWARF_PATH + "Axeman")]
public class Axeman : UnitData
{
    [TitleGroup("Settings")]
    [Tooltip("Amount healed when spawned near other axeman")]
    [Range(1, 5)]
    [SerializeField]
    private int spawnHealing = 1;

    public override UnitSide UnitSide => UnitSide.Dwarf;
    public override UnitKind UnitKind => UnitKind.Axeman;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    protected override IEnumerator SummonStep (SummonStepSettings settings)
    {
        var axemanAroundMyself = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(b => b.HasUnitOnIt && b.UnitData.UnitKind == UnitKind.Axeman)
            .ToList();

        if (axemanAroundMyself.Any())
        {
            foreach (var blockData in axemanAroundMyself)
                yield return blockData.ApplyHealing(new BlockData.ApplyHealingSettings() { healing = spawnHealing });

            yield return settings.SummonedBlock.ApplyHealing(
                new BlockData.ApplyHealingSettings() { healing = spawnHealing });
        }

        yield return base.SummonStep(settings);
    }

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

        yield return randomizedBlock.ApplyDamage(new BlockData.ApplyDamageSettings() { damage = Damage });
    }
}