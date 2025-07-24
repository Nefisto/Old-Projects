using UnityEngine;

public static class GameCamera
{
    private static Camera cachedHudCamera;

    public static Camera HudCamera
        => cachedHudCamera ??= GameObject
            .FindGameObjectWithTag("HUDCamera")
            .GetComponent<Camera>();
}