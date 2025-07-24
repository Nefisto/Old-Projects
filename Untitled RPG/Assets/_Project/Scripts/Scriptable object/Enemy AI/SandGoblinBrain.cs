using Loot;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Sand goblin brain", menuName = EditorConstants.MenuAssets.ENEMY_BRAIN + "Sand goblin",
    order = 0)]
public class SandGoblinBrain : EnemyBrain
{
    [TitleGroup("Settings", order: -15)]
    [Range(0f, 1f)]
    [SerializeField]
    private float healthThresholdToEscape = .15f;

    [TitleGroup("Settings")]
    [SerializeField]
    private int turnsBetweenDebuffCheck = 3;

    [TitleGroup("Skills")]
    [SerializeField]
    private SandThrown sandThrown;

    [TitleGroup("Skills")]
    [SerializeField]
    private Steal stealSkill;

    [TitleGroup("Skills")]
    [SerializeField]
    private EscapeBattle escapeSkill;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private int turnsWithoutCheckForDebuffs = 0;

    public override void ActUpdate()
    {
        turnsWithoutCheckForDebuffs++;
        if (turnsWithoutCheckForDebuffs > turnsBetweenDebuffCheck && skillNameToRule[sandThrown.Name].IsDisabled)
            skillNameToRule[sandThrown.Name].IsDisabled = false;
    }

    public override Drop[] GetBehaviorRules (BehaviorContext context)
    {
        var stealSkillInstance = (Steal)stealSkill.GetInstance;
        var stealDrop = new Drop
        {
            Entry = stealSkillInstance,
            Weight = GameConstants.SKILL_WEIGHT_MID
        };
        stealSkillInstance.OnSuccessfullyStole += _ => stealDrop.IsDisabled = true;

        var sandThrownInstance = (SandThrown)sandThrown.GetInstance;
        var sandThrownDrop = new Drop
        {
            Entry = sandThrownInstance,
            Weight = GameConstants.SKILL_WEIGHT_HIGH
        };
        sandThrownInstance.OnCast += () =>
        {
            sandThrownDrop.IsDisabled = true;
            turnsWithoutCheckForDebuffs = 0;
        };
        skillNameToRule.Add(sandThrownInstance.Name, sandThrownDrop);

        var battleActor = context.battleActor;
        var escapeDrop = new Drop
        {
            Entry = escapeSkill,
            Weight = GameConstants.SKILL_WEIGHT_VERY_HIGH,
            IsDisabled = true
        };
        battleActor.HealthResource.OnUpdatedCurrent += (_, _)
            => escapeDrop.IsDisabled = battleActor.HealthResource.CurrentPercentage > healthThresholdToEscape;

        var attackDrop = new Drop
        {
            Entry = context.enemyData.DefaultAttackSkill,
            Weight = GameConstants.SKILL_WEIGHT_LOW
        };

        return new[] { attackDrop, stealDrop, sandThrownDrop, escapeDrop };
    }
}