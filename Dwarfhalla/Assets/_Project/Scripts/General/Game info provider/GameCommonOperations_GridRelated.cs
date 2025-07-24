using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;
using UnityEngine.Assertions;

public static partial class CommonOperations
{
    public static bool CheckForFreeViewBetween (Vector2Int a, Vector2Int b)
    {
        var hasFreeView = true;
        // As we are starting FROM and going TO, we do an A-B instead of B-A
        var vectorAB = new Vector2Int(b.x - a.x, b.y - a.y);
        var unitaryValue = new Vector2Int
        (vectorAB.x == 0 ? 0 : vectorAB.x / Mathf.Abs(vectorAB.x),
            vectorAB.y == 0 ? 0 : vectorAB.y / Mathf.Abs(vectorAB.y));
        var currentPosition = a + unitaryValue;

        var validationCounter = 0;
        while (currentPosition.x != b.x || currentPosition.y != b.y)
        {
            if (GetBlockDataAt(currentPosition.x, currentPosition.y).HasUnitOnIt)
            {
                hasFreeView = false;
                break;
            }

            currentPosition += unitaryValue;

            validationCounter++;

            Assert.IsTrue(validationCounter < 15, "Infinite loop caused when checking for free view");
        }

        return hasFreeView;
    }

    public static RoomData GetCurrentRoom()
        => ServiceLocator
            .GameContext.LevelData.CurrentRoom;


    public static void CancelNotificationOnAllGrid()
    {
        foreach (var runningPreview in RunningPreviews)
            runningPreview?.Stop();
        RunningPreviews.Clear();

        GameContext
            .LevelData
            .CurrentRoom
            .Select(b => b.RuntimeBlock)
            .ForEach(r => r.CancelNotification());
    }

    public static IEnumerable<BlockData> GetBlockDataAt (IEnumerable<Vector2Int> positions)
        => positions
            .Select(GetBlockDataAt);

    public static BlockData GetBlockDataAt (int x, int y)
        => GameContext
            .LevelData
            .CurrentRoom
            .GetBlock(x, y);

    public static BlockData GetBlockDataAt (Vector2Int possiblePosition)
        => GetBlockDataAt(possiblePosition.x, possiblePosition.y);

    /// <summary>
    /// Multiple pieces do some side effects on near pieces, like, attack at (1, 1) and causes half damage on behind enemy.
    ///     This api will support this kind of operation
    /// </summary>
    public static BlockData GetNearBlockAtDirection (BlockData randomizedBlock, Vector2Int attackDirection)
    {
        return GetBlockDataAt(new Vector2Int(randomizedBlock.Position.x + attackDirection.x,
            randomizedBlock.Position.y + attackDirection.y));
    }
}