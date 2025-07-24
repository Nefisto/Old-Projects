using System.Collections.Generic;
using UnityEngine;

public partial class RoomData : IEnumerable<BlockData>
{
    public RoomData (int width, int height)
    {
        RoomSize = new Vector2Int(width, height);

        Grid = new BlockData[width, height];
        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
            Grid[i, j] = new BlockData { Position = new Vector2Int(i, j) };
    }

    public Vector2Int RoomSize { get; set; }

    private BlockData[,] Grid { get; }

    public BlockData GetBlock (int x, int y) => !IsCoordinatesInsideGrid(x, y) ? null : Grid[x, y];

    private bool IsCoordinatesInsideGrid (int x, int y)
        => x >= 0
           && x < Grid.GetLength(0)
           && y >= 0
           && y < Grid.GetLength(1);
}