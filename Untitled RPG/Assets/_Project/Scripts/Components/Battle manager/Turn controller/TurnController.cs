using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class TurnController : MonoBehaviour, IList<BattleActionContext>
{
    [Title("Settings")]
    [ReadOnly]
    [SerializeField]
    private List<BattleActionContext> waitingActors;

    [TitleGroup("Debug")]
    [SerializeField]
    private List<BattleActionContext> actionHistory;

    public static EntryPoint<BattleActionContext> OnRunningAction { get; set; } = new();
    public static EntryPoint<BattleActionContext> OnRanAction { get; set; } = new();

    private void Awake()
    {
        ServiceLocator.TurnController = this;

        BattleManager.OnSettingUpBattle += SettingUpForBattle;

        actionHistory.Clear();
        GameEvents.onRunBattleAction += ctx => actionHistory.Insert(0, ctx);
    }

    private void SettingUpForBattle()
    {
        Clear();
        OnRunningAction.Clear();
        OnRanAction.Clear();
        actionHistory.Clear();
    }

    public IEnumerator RunQueuedActions()
    {
        if (Count == 0)
            yield break;

        foreach (var actionInfo in this.OrderBy(a => a.caster.BattleActorSide))
        {
            if (actionInfo.caster.IsActorDead())
                continue;

            yield return OnRunningAction?.YieldableInvoke(actionInfo);

            foreach (var info in actionInfo)
                foreach (var statusEffect in info.caster.StatusEffectController)
                    yield return statusEffect.ApplyOnCurrentAction(info);

            yield return actionInfo.skill.Run(actionInfo);
            yield return SpendManaPhase(actionInfo);

            yield return OnRanAction?.YieldableInvoke(actionInfo);
            actionInfo.OnActionCast?.Invoke();
        }

        Clear();
    }

    private IEnumerator SpendManaPhase (BattleActionContext battleActionContext)
    {
        if (battleActionContext.skill.SyncedResource is null)
            yield break;

        if (battleActionContext.skill.ResourceCost == 0)
            yield break;

        // Before spend mana event

        battleActionContext.skill.SyncedResource.Current -= battleActionContext.skill.ResourceCost;

        // After mana cast event
    }
}