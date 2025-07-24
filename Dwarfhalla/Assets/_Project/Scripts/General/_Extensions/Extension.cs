using UnityEngine;

public static partial class Extension
{
    private static Camera hudCachedCamera;

    public static Camera HudCamera (this Camera camera)
        => hudCachedCamera ??= GameObject
            .FindGameObjectWithTag("HUDCamera")
            .GetComponent<Camera>();
}