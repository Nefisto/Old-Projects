using UnityEngine;

public class Vector2IntHelper
{
    public static bool IsInRange (Vector2Int vector, int value, bool includeMaximum = true)
    {
        if (!includeMaximum)
            vector.y--;

        return value >= vector.x && value <= vector.y;
    }
}