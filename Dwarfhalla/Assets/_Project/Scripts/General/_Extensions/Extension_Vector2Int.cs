using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Extension
{
    public static IEnumerable<BlockData> GetBlocksFromPositions (this IEnumerable<Vector2Int> positions)
        => CommonOperations.GetBlockDataAt(positions);

    public static Vector2Int GetDirectionTo (this Vector2Int originalPosition, Vector2Int targetPosition)
    {
        var normalizedVector = ((Vector2)(targetPosition - originalPosition)).normalized;
        return new Vector2Int(Mathf.RoundToInt(normalizedVector.x), Mathf.RoundToInt(normalizedVector.y));
    }

    public static Dictionary<Vector2Int, List<Vector2Int>> ToDirectForPosition(this IEnumerable<Vector2Int> positions, Vector2Int origin)
    {
        var result = new Dictionary<Vector2Int, List<Vector2Int>>();
        
        foreach (var position in positions)
        {
            var direction = position - origin;
            direction.x = Mathf.Clamp(direction.x, -1, 1);
            direction.y = Mathf.Clamp(direction.y, -1, 1);
            
            result.TryAdd(direction, new List<Vector2Int>());
            result[direction].Add(position);
        }

        return result;
    }

    /// <summary>
    /// Board directions are coming into 2d coordinates, but when moving piece we need to chagne the Y coordinate to Z
    /// </summary>
    public static Vector3 ToXZY (this Vector2Int point) => new(point.x, 0, point.y);

    public static Vector2Int UpRight (this Vector2Int point) => new(1, 1);
    public static Vector2Int DownRight (this Vector2Int point) => new(1, -1);
    public static Vector2Int DownLeft (this Vector2Int point) => new(-1, 1);
    public static Vector2Int UpLeft (this Vector2Int point) => new(-1, 1);
}