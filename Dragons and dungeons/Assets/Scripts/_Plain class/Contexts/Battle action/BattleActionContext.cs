using System.Collections.Generic;

/// <summary>
/// Context necessary to run a battle action
/// </summary>
public class BattleActionContext
{
    /// <summary>
    /// The action caster 
    /// </summary>
    public BattleActor caster;

    /// <summary>
    /// Skill that will be cast
    /// </summary>
    public Skill skill;

    /// <summary>
    /// Targets list for this action
    /// </summary>
    public List<BattleActor> targets = new List<BattleActor>();

    public int originalActionValue;
    /// <summary>
    /// Amount of damage/heal that this action will make 
    /// </summary>
    public int actionValue;

    /// <summary>
    /// Original caster side, will be used as reference to apply some debuffs
    /// </summary>
    private readonly BattleActorSide originalSide;

    /// <summary>
    /// Necessary cause some stats can change drastically this data.
    /// e.g. An AllFriendlyButMe healing can transform into an AllEnemies if confused 
    /// </summary>
    private SkillGroupTarget originalGroupTarget => skill.groupTarget;

    /// <summary>
    /// Necessary cause some stats can change drastically this data.
    /// e.g. An AllFriendlyButMe healing can transform into an AllEnemies if confused 
    /// </summary>
    public BattleActorSide ActualCurrentSide => originalSide;

    /// <summary>
    /// Necessary cause some stats can change drastically this data.
    /// e.g. An AllFriendlyButMe healing can transform into an AllEnemies if confused 
    /// </summary>
    public SkillGroupTarget ActualCurrentGroupTarget => originalGroupTarget;
}