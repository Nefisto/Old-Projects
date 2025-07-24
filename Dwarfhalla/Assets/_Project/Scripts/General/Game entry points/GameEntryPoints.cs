using System;
using NTools;

public static class GameEntryPoints
{
    public static EntryPoint<object> OnLoadAssets { get; set; } = new();
    public static EntryPoint<object> GeneratingSessionData { get; set; } = new();
    public static EntryPoint<object> OnRenderingLevel { get; set; } = new();
    public static EntryPoint<object> OnFinishedSetup { get; set; } = new();
    public static EntryPoint<object> OnRenderedLevel { get; set; } = new();
    public static EntryPoint<object> OnGeneratedPlayerData { get; set; } = new();

    public static EntryPoint<object> OnSetupScene { get; set; } = new();
    public static EntryPoint<object> OnPlayerPerformedAction { get; set; } = new();

    public static EntryPoint<object> OnGoblinDied { get; set; } = new();

    public static EntryPoint<object, EventArgs> OnSelectedReward { get; set; } = new();
}