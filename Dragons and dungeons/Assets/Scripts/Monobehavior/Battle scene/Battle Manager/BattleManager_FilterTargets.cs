using System;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

// You're probably thinking - WHY DA FU** THIS NOOB IS USING GET COMPONENT EVERY TIME INSTEAD OF CREATE ENEMIES/FRIENDS GROUPS ON START -
// I've chose this approach case at moment IDK if new actors will be added to the battle in runtime, think in summons for example,
// so getting objects each time will ensure that this will ALWAYS give the right targets, also this isn't a thing that happen every frame
// only after every action, so its not a big deal


/// This part will filter targets and supply APIs to allow requests it.
/// Friends and enemies is based on runtime modifiers and reference.
/// e.g. The enemy of a monster is the player, the friend of a confused player is an enemy
public partial class BattleManager
{
    [TabGroup("Filter", true)]
    [Title("Control")]
    [SerializeField]
    private RuntimeSet battleActorGroup;
    
    public List<BattleActor> GetPlayerCharacters()
        => GetAllFriendsTargets(BattleActorSide.Player).ToList();

    public List<BattleActor> GetEnemiesCharacters()
        => GetAllEnemiesTargets(BattleActorSide.Player).ToList();
    
    /// <summary>
    /// In the next group of methods we are getting friends and enemies based on runtime modifiers and reference.
    /// e.g. The enemy of a monster is the player, the friend of a confused player is an enemy
    /// </summary>
    public List<BattleActor> GetTargets (BattleActionContext actionContext)
    {
        var side = actionContext.ActualCurrentSide;
        var groupTarget = actionContext.ActualCurrentGroupTarget;

        var targets = groupTarget switch
        {
            SkillGroupTarget.Selectable => GetSingleTarget(side),
            SkillGroupTarget.Single => GetSingleTarget(side),
            SkillGroupTarget.Myself => new[] { actionContext.caster },
            SkillGroupTarget.Everyone => GetAllTargets(),
            SkillGroupTarget.EveryoneButMe => GetAllTargetButMe(actionContext.caster),
            SkillGroupTarget.Friendly => GetAllFriendsTargets(side),
            SkillGroupTarget.FriendlyButMe => GetAllFriendsTargetsButMe(side, actionContext.caster),
            SkillGroupTarget.AllEnemies => GetAllEnemiesTargets(side),
            _ => throw new Exception("Not a valid SKILL GROUP TARGET")
        };

        return targets.ToList();
    }

    private IEnumerable<BattleActor> GetSingleTarget (BattleActorSide side)
        => GetAllEnemiesTargets(side)
            .NTGetRandom(1);

    private IEnumerable<BattleActor> GetAllTargets ()
        => battleActorGroup
            .items
            .Select(item => item.GetComponent<BattleActor>());

    public IEnumerable<BattleActor> GetAllTargetButMe (BattleActor me)
        => GetAllTargets()
            .Where(battleActor => battleActor != me);

    private IEnumerable<BattleActor> GetAllEnemiesTargets (BattleActorSide side)
        => GetAllTargets()
            .Where(battleActor => battleActor.OriginalSide != side);

    private IEnumerable<BattleActor> GetAllFriendsTargets (BattleActorSide side)
        => GetAllTargets()
            .Where(battleActor => battleActor.OriginalSide == side);

    private IEnumerable<BattleActor> GetAllFriendsTargetsButMe (BattleActorSide side, BattleActor me)
        => GetAllFriendsTargets(side)
            .Where(battleActor => battleActor != me);
}