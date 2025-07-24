public static partial class ServiceLocator
{
    private static BattleContext battleContext = new();

    public static BattleContext BattleContext
    {
        get => battleContext ??= new BattleContext();
        set => battleContext = value;
    }

    public static TurnController TurnController { get; set; }
    public static TargetController TargetController { get; set; }

    public static SkillAnimationPooler SkillVFX { get; set; }
    public static BattleLogMessagePooler BattleLogPooler { get; set; }
}