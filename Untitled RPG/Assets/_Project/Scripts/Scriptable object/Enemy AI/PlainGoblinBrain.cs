using Loot;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Plain goblin brain", menuName = EditorConstants.MenuAssets.ENEMY_BRAIN + "Plain goblin",
    order = 0)]
public class PlainGoblinBrain : EnemyBrain
{
    [TitleGroup("Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float healthThresholdToEscape = .6f;

    [TitleGroup("Settings", order: -5)]
    [SerializeField]
    private Steal stealSkill;

    [TitleGroup("Settings")]
    [SerializeField]
    private EscapeBattle escapeSkill;

    public override Drop[] GetBehaviorRules (BehaviorContext context)
    {
        var battleActor = context.battleActor;
        var stealSkillInstance = (Steal)stealSkill.GetInstance;
        var stealDrop = new Drop
        {
            Entry = stealSkillInstance,
            Weight = GameConstants.SKILL_WEIGHT_MID
        };
        stealSkillInstance.OnSuccessfullyStole += _ => stealDrop.IsDisabled = true;

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

        return new[] { attackDrop, stealDrop, escapeDrop };
    }
}