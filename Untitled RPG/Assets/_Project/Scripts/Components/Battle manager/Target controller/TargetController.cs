using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private void Awake() => ServiceLocator.TargetController = this;

    public IEnumerable<BattleActor> GetTargets (GroupTarget groupTarget, BattleActor caster)
        => groupTarget switch
        {
            GroupTarget.SingleEnemy => GetSingleEnemyTarget(caster.BattleActorSide),
            GroupTarget.FriendlyGroup => GetAllAllies(caster.BattleActorSide),
            GroupTarget.MySelf => new List<BattleActor> { caster },
            GroupTarget.AllEnemies => GetAllEnemies(caster.BattleActorSide),
            _ => throw new NotImplementedException()
        };

    private IEnumerable<BattleActor> GetSingleEnemyTarget (BattleActorSide side)
        => GetAllTargets()
            .Where(ba => ba.BattleActorSide != side)
            .Take(1);

    private IEnumerable<BattleActor> GetAllAllies (BattleActorSide side)
        => GetAllTargets()
            .Where(ba => ba.BattleActorSide == side);

    private IEnumerable<BattleActor> GetAllEnemies (BattleActorSide side)
        => GetAllTargets()
            .Where(ba => ba.BattleActorSide != side);

    private IEnumerable<BattleActor> GetAllTargets()
        => ServiceLocator
            .BattleContext
            .AllBattleActor
            .Select(entry => entry.GetComponent<BattleActor>());
}