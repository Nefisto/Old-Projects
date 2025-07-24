using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

/// <summary>
///     Common operations used to simplify some calculations
/// </summary>
public static partial class CommonOperations
{
    public static List<NTask> RunningPreviews { get; set; } = new();

    private static GameContext GameContext => ServiceLocator.GameContext;

    public static List<BlockData> GetAllBlocksOnCurrentRoom()
        => ServiceLocator
            .GameContext
            .LevelData
            .CurrentRoom
            .ToList();

    public static List<BlockData> GetAllFreeBlocksOnCurrentRoom()
        => ServiceLocator
            .GameContext
            .LevelData
            .CurrentRoom
            .Where(bd => !bd.HasUnitOnIt)
            .ToList();

    public static IEnumerable<RuntimeBlock> GetAllRuntimeBlocksOnCurrentRoom()
        => ServiceLocator
            .GameContext
            .LevelData
            .CurrentRoom
            .Select(b => b.RuntimeBlock);

    public static IEnumerator ProcessDeathAnimation()
    {
        var counter = 0;
        var required = GameManager.DeadUnits.Count;
        foreach (var unit in GameManager.DeadUnits)
        {
            var task = new NTask(unit.DieAnimation());
            task.OnFinished += _ => counter++;
        }

        yield return new WaitUntil(() => counter >= required);

        GameManager.DeadUnits.Clear();
    }

    public static IEnumerable<Vector2Int> GetVisibleBlocks (PatternGetSettings settings,
        Func<PatternGetSettings, IEnumerable<Vector2Int>> patternGetter)
        => patternGetter
            .Invoke(settings)
            .Where(targetPosition => CheckForFreeViewBetween(settings.position, targetPosition))
            .Select(b => b);
}