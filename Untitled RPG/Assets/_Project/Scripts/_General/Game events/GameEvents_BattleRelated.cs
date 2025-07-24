using System;
using NTools;

public partial class GameEvents
{
    public static Action<Skill> onChangedSkill;
    public static Action<OnSkillButtonPressedContext> onSkillButtonPressed;
    public static Action<BattleActionContext> onRunBattleAction;
    public static Action<BattleSetupContext> onBattleTriggered;

    public static EntryPoint<BattleResultData> OnBattleFinishingdEntryPoint { get; set; } = new();
    public static EntryPoint<BattleSetupContext> OnBattleFinishedEntryPoint { get; set; } = new();
}