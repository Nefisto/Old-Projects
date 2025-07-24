using UnityEngine;

public static partial class ServiceLocator
{
    private static IBattleResultHUD battleResultHUD = new NullBattleResultHUD();

    public static IBattleResultHUD BattleResultHUD
    {
        get => battleResultHUD ??= new NullBattleResultHUD();
        set => battleResultHUD = value;
    }

    public static BattleResultData BattleResulData { get; set; } = ScriptableObject.CreateInstance<BattleResultData>();
}