using System;
using NTools;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class ConfirmTargetsButton : LazyBehavior
{
    private BattleManager battleManager;
    private Func<bool> canInteractWithButton;
    
    private void Awake()
        => battleManager = BattleManager.Instance;

    private void OnDisable()
    {
        battleManager.OnAddedTarget -= ListenerForChangeTargetAmount;
        battleManager.OnRemoveTarget -= ListenerForChangeTargetAmount;
    }

    public void SetupButton(BattleActionContext ctx)
    {
        battleManager.OnAddedTarget += ListenerForChangeTargetAmount;
        battleManager.OnRemoveTarget += ListenerForChangeTargetAmount;

        canInteractWithButton = () => battleManager.SelectedTargets.Count != 0 
                                      && battleManager.SelectedTargets.Count <= ctx.skill.maxAmountOfTargets;
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            ((FriendlyBattleActor)ctx.caster).CompleteTargetSelection();
        });

        button.interactable = false;
    }

    private void ListenerForChangeTargetAmount (BattleActor _)
        => RefreshInteractState();

    private void RefreshInteractState()
        => button.interactable = canInteractWithButton.Invoke();
}