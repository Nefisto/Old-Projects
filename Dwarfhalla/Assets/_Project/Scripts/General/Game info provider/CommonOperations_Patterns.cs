using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class CommonOperations
{
    public static List<Vector2Int> GetAroundPatternPositionsFrom (PatternGetSettings settings)
        => GetAroundPatternPositionsFrom(settings.position);

    public static List<Vector2Int> GetCrossPatternFrom (PatternGetSettings settings)
        => GetCrossPatternFrom(settings.position, settings.range);

    public static IEnumerable<Vector2Int> GetPlusAndCrossPatterFrom (PatternGetSettings settings)
        => GetPlusAndCrossPatterFrom(settings.position, settings.range);


    public static IEnumerable<Vector2Int> GetHorizontallyPatternDividedByDirectionsFrom (PatternGetSettings settings)
        => GetHorizontallyPatternDividedByDirectionsFrom(settings.position.x, settings.position.y, settings.range)
            .SelectMany(t => t.Value.Select(bd => bd.Position));

    public static List<Vector2Int> GetAroundPatternPositionsFrom (Vector2Int position)
        => GetAroundPatternPositionsFrom(position.x, position.y);

    /// <summary>
    ///     Around target piece in a O pattern
    /// </summary>
    public static List<Vector2Int> GetAroundPatternPositionsFrom (int initialX, int initialY)
    {
        var result = new List<Vector2Int>();
        var (x, y) = (initialX, initialY);

        (x, y) = (initialX - 1, initialY);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        (x, y) = (initialX - 1, initialY + 1);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        (x, y) = (initialX, initialY + 1);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        (x, y) = (initialX + 1, initialY + 1);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        (x, y) = (initialX + 1, initialY);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        (x, y) = (initialX + 1, initialY - 1);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        (x, y) = (initialX, initialY - 1);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        (x, y) = (initialX - 1, initialY - 1);
        if (IsValidPosition(x, y))
            result.Add(new Vector2Int(x, y));

        return result;
    }

    public static List<Vector2Int> GetPlusAndCrossPatterFrom (Vector2Int position, int range = 2)
    {
        var result = new List<Vector2Int>();

        result.AddRange(GetPlusPatternFrom(position, range));
        result.AddRange(GetCrossPatternFrom(position, range));

        return result;
    }

    public static List<Vector2Int> GetPlusPatternFrom (Vector2Int position, int range = 2)
        => GetPlusPatternFrom(position.x, position.y, range);

    public static List<Vector2Int> GetCrossPatternFrom (Vector2Int position, int range = 2)
        => GetCrossPatternFrom(position.x, position.y, range);

    public static List<Vector2Int> GetPlusPatternFrom (int x, int y, int range = 2)
    {
        var directions = new Vector2Int[] { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

        return GetStraightLineInDirectionFrom(x, y, range, directions);
    }

    public static List<Vector2Int> GetCrossPatternFrom (int x, int y, int range = 2)
    {
        var directions = new Vector2Int[] { new(1, 1), new(-1, 1), new(1, -1), new(-1, -1) };

        return GetStraightLineInDirectionFrom(x, y, range, directions);
    }

    public static Dictionary<Vector2Int, List<BlockData>>
        GetCrossPatternDividedByDirectionsFrom (Vector2Int position, int range = 2)
        => GetCrossPatternDividedByDirectionsFrom(position.x, position.y, range);

    public static Dictionary<Vector2Int, List<BlockData>> GetCrossPatternDividedByDirectionsFrom (int x, int y,
        int range = 2)
        => GetBlocksFromDirections(x, y, range, new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1, 1));

    public static Dictionary<Vector2Int, List<BlockData>> GetPlusPatternDividedByDirectionsFrom (int x, int y,
        int range = 2)
        => GetBlocksFromDirections(x, y, range,
            Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down);

    public static Dictionary<Vector2Int, List<BlockData>>
        GetCrossAndPlusDictionaryDivideDictionaryByDirectionsFrom (Vector2Int position, int range = 2)
        => GetBlocksFromDirections(position.x, position.y, range,
            Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down, new Vector2Int(1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1, 1));

    public static Dictionary<Vector2Int, List<BlockData>> GetHorizontallyPatternDividedByDirectionsFrom (int x, int y,
        int range = 2)
        => GetBlocksFromDirections(x, y, range, Vector2Int.right, Vector2Int.left);

    private static Dictionary<Vector2Int, List<BlockData>> GetBlocksFromDirections (int x, int y, int range = 2,
        params Vector2Int[] directions)
    {
        var result = new Dictionary<Vector2Int, List<BlockData>>();

        foreach (var direction in directions)
        {
            var blocksInThisDirection = GetStraightLineInDirectionFrom(x, y, range, direction);

            if (blocksInThisDirection == null)
                continue;

            result.Add(direction, GetBlockDataAt(blocksInThisDirection).ToList());
        }

        return result;
    }

    private static List<Vector2Int> GetStraightLineInDirectionFrom (int x, int y, int range = 2,
        params Vector2Int[] directions)
    {
        var result = new List<Vector2Int>();
        foreach (var direction in directions)
        {
            var currentPosition = new Vector2Int(x, y);

            for (var i = 0; i < range; i++)
            {
                currentPosition += direction;

                if (!IsValidPosition(currentPosition.x, currentPosition.y))
                    break;

                result.Add(new Vector2Int(currentPosition.x, currentPosition.y));
            }
        }

        return result;
    }

    private static bool IsValidPosition (int x, int y)
    {
        var gridSize = GetCurrentRoom().RoomSize;
        var blockOnPosition = GetBlockDataAt(x, y);

        var isInsideGrid = x >= 0 && y >= 0 && x < gridSize.x && y < gridSize.y;
        return isInsideGrid && blockOnPosition != null;
    }
}