using NTools;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class CancelTargetSelectionButton : LazyBehavior
{
    public void SetupButton(BattleActionContext ctx)
    {
        var caster = ctx.caster;
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            ((FriendlyBattleActor)caster).CancelSelection();
        });
    }
}