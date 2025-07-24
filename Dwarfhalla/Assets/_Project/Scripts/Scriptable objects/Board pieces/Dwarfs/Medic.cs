using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Medic", menuName = EditorConstants.DWARF_PATH + "Medic")]
public class Medic : UnitData
{
    public static Dictionary<UnitData, int> CachedHealing = new();
    public override UnitSide UnitSide => UnitSide.Dwarf;
    public override UnitKind UnitKind => UnitKind.Medic;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    private IEnumerable<BlockData> GetDamageBlocks (Vector2Int position)
        => GetDamagePattern(new PatternGetSettings { position = position })
            .GetBlocksFromPositions();

    private IEnumerable<BlockData> GetHealingBlocks (Vector2Int position)
        => CommonOperations
            .GetAroundPatternPositionsFrom(position)
            .GetBlocksFromPositions();

    public override IEnumerator PreviewAttack (PreviewSettings settings = null)
    {
        var correctPosition = settings?.CustomPosition ?? PiecePosition;
        var damageBlocks = GetDamagePattern(new PatternGetSettings { position = correctPosition })
            .GetBlocksFromPositions()
            .ToList();
        var healingBlocks = GetHealingBlocks(correctPosition).ToList();

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

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var randomizedEnemy = GetDamagePattern(new PatternGetSettings { position = PiecePosition })
            .GetBlocksFromPositions()
            .Where(b => b.HasGoblinUnit)
            .Shuffle()
            .FirstOrDefault();

        var healingBlocks = GetHealingBlocks(PiecePosition)
            .Where(b => b.HasDwarfUnit);

        foreach (var healingBlock in healingBlocks)
            yield return HealingHandle(healingBlock);

        if (randomizedEnemy != null)
            yield return randomizedEnemy.ApplyDamage(new BlockData.ApplyDamageSettings { damage = Damage });
    }

    private IEnumerator HealingHandle (BlockData healingBlock)
    {
        var targetHealing = healingBlock.UnitData;
        if (CachedHealing.TryGetValue(targetHealing, out var healedAmount) && healedAmount >= 3)
            yield break;

        yield return healingBlock.ApplyHealing(new BlockData.ApplyHealingSettings() { healing = 1 });

        if (!targetHealing.HasExtraLife)
            yield break;

        if (CachedHealing.TryAdd(targetHealing, healedAmount))
            targetHealing.OnTookDamage += _
                => CachedHealing[targetHealing] = Mathf.Max(CachedHealing[targetHealing] - Damage, 0);

        CachedHealing[targetHealing]++;
    }
}