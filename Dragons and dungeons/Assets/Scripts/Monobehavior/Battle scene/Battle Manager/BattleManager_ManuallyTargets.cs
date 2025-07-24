using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public partial class BattleManager
{
    public event Action<BattleActor> OnAddedTarget;
    public event Action<BattleActor> OnRemoveTarget;

    [FormerlySerializedAs("background")]
    [TabGroup("Target routine")]
    [Title("Control")]
    [Tooltip("Black background that appear when the select target routine start")]
    [SerializeField]
    private GameObject semiTransparentBackground;

    [TabGroup("Target routine")]
    [SerializeField]
    private ConfirmTargetsButton confirmButton;

    [TabGroup("Target routine")]
    [SerializeField]
    private CancelTargetSelectionButton cancelSelectionButton;

    [TabGroup("Target routine")]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private int maxAmountOfTargets;

    [TabGroup("Target routine")]
    [ReadOnly]
    [SerializeField]
    private List<BattleActor> selectedTargets;

    public List<BattleActor> SelectedTargets => selectedTargets;

    public bool TryAddTarget (BattleActor target)
    {
        if (selectedTargets.Contains(target))
        {
            Debug.LogError("System is trying to add the same target multiple times", this);
            return false;
        }

        if (!CanAddTarget())
            return false;

        selectedTargets.Add(target);
        OnAddedTarget?.Invoke(target);
        return true;
    }

    public void RemoveTarget (BattleActor target)
    {
        if (!selectedTargets.Contains(target))
        {
            Debug.LogError("System is remove a target that isn't being added", this);
            return;
        }

        selectedTargets.Remove(target);
        OnRemoveTarget?.Invoke(target);
    }

    public bool ContainAsTarget (BattleActor target)
        => selectedTargets.Contains(target);

    public IEnumerator AllowActorsToBeTargeted (BattleActionContext ctx)
    {
        SetMaxAmountOfTargets(ctx.skill);
        EnableBackground();
        ClearSelectedTargets();
        EnableConfirmButton();
        SetupConfirmButton(ctx);
        EnableCancelButton();
        SetupCancelButton(ctx);

        foreach (var battleActor in battleActorGroup.items.Select(i => i.GetComponent<BattleActor>()))
            if (battleActor is ITargetable targetable)
                targetable.EnableTarget();

        yield return null;
    }

    public IEnumerator DisallowActorsToBeTargeted()
    {
        DisableBackground();
        DisableConfirmButton();
        DisableCancelButton();

        foreach (var battleActor in battleActorGroup.items.Select(i => i.GetComponent<BattleActor>()))
            if (battleActor is ITargetable targetable)
                targetable.DisableTarget();

        yield return null;
    }

    private void DisableCancelButton()
        => cancelSelectionButton.gameObject.SetActive(false);

    private void DisableConfirmButton()
        => confirmButton.gameObject.SetActive(false);

    private void EnableCancelButton()
        => cancelSelectionButton.gameObject.SetActive(true);

    private void EnableConfirmButton()
        => confirmButton.gameObject.SetActive(true);

    private void SetupCancelButton (BattleActionContext ctx)
        => cancelSelectionButton.SetupButton(ctx);

    private void SetupConfirmButton (BattleActionContext ctx)
        => confirmButton.SetupButton(ctx);

    private bool CanAddTarget()
        => selectedTargets.Count < maxAmountOfTargets;

    private void SetMaxAmountOfTargets (Skill skill)
        => maxAmountOfTargets = skill.maxAmountOfTargets;

    private void ClearSelectedTargets()
        => selectedTargets.Clear();

    private void EnableBackground()
        => semiTransparentBackground.SetActive(true);

    private void DisableBackground()
        => semiTransparentBackground.SetActive(false);
}