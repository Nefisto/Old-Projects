using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public partial class FriendlyBattleActor : BattleActor
{
    [TabGroup("Data")]
    [Title("Control")]
    [Tooltip("Which panel is vinculate with this character")]
    [SerializeField]
    private GameObject actionPanelPrefab;

    [TabGroup("Data")]
    [Tooltip("Position to instantiate")]
    [SerializeField]
    private Transform actionPanelFolder;
    
    [TabGroup("Data")]
    [Title("Debug")]
    [Tooltip("Action panel reference")]
    [ReadOnly]
    [SerializeField]
    private ActionPanel actionPanel;

    private bool hasCanceledSelection = false;

    [Button]
    public void Setup (PlayableCharacterData data)
    {
        base.Setup(data);
        
        ConfigureActionPanel();

        Data.OnChangeEquipment += OnChangeEquipmentListener;
        RedrawEquipment();
    }

    private void OnChangeEquipmentListener()
    {
        hasCanceledSelection = true;
        DisableSkills();

        RedrawEquipment();
    }

    public override IEnumerator StartTurn()
    {
        yield return base.StartTurn();
        
        RefreshActionPanel();
        actionPanel.ShowPanel();

        RaiseFinishStartTurn(new StartTurnContext()
        {
            Actor = this,
            InventoryData = inventory
        });
    }

    public override IEnumerator RunTurn()
    {
        yield return base.RunTurn();

        do
        {
            hasCanceledSelection = false;
            yield return new WaitUntil(PlayerChoseASkill);
            if (turnContext.skill.groupTarget != SkillGroupTarget.Selectable)
                yield return base.SetTargets();
            else
            {
                yield return BattleManager.Instance.AllowActorsToBeTargeted(turnContext);
                yield return new WaitUntil(PlayerChoseTargetsOrCancelSelection);
                yield return BattleManager.Instance.DisallowActorsToBeTargeted();
            }
        } while (hasCanceledSelection);

        yield return RunSkill(turnContext.skill);
    }

    public void SkipTurn()
        => turnContext.skill = Skip.Instance;

    [Button]
    public void DisableSkills()
        => actionPanel.DisableAllSkills();

    public void CancelSelection()
    {
        hasCanceledSelection = true;
        turnContext.skill = null;
        actionPanel.ToggleSkillBasedOnMana(Mana);
        actionPanel.ShowPanel();
    }

    private bool PlayerChoseTargetsOrCancelSelection()
        => HasPlayerChosedATarget() || HasPlayerCanceledSelection();

    private bool HasPlayerCanceledSelection()
        => hasCanceledSelection;

    #region Transfer control to UI

    /*
     * Unity does not have a built in way to transfer control from a coroutine to UI, so those methods are used to make it
     */
    private bool HasPlayerChosedATarget()
        => turnContext.targets.Count != 0;

    public void CompleteTargetSelection()
        => turnContext.targets = BattleManager.Instance.SelectedTargets.ToList();
    
    private bool PlayerChoseASkill()
        => turnContext.skill != null;

    public void ChoseTarget (BattleActor target)
        => turnContext.targets = new List<BattleActor>() { target };

    public void ChoseSkill (Skill skill)
        => turnContext.skill = skill;

    #endregion

    public override IEnumerator RunSkill (Skill skill)
    {
        RaiseRunSkill(new RunSkillContext(){skill = skill});
        
        actionPanel.DisablePanel();
        PayForSkill(skill);
        
        return base.RunSkill(skill);
    }

    public override void ApplyModifier (Modifier modifier)
    {
        base.ApplyModifier(modifier);

        actionPanel.RefreshModifiers(Modifiers);
    }

    private void ConfigureActionPanel()
    {
        actionPanel = CreateActionPanel();

        RefreshActionPanel();
        var ctx = new UpdateActionPanelContext(this, Data.GetValidActiveSkills());
        actionPanel.ConfigureSkipButton(ctx);
        actionPanel.SetOwner(ctx);

        actionPanel.DisablePanel();
    }

    private void RefreshActionPanel()
    {
        var ctx = new UpdateActionPanelContext(this, Data.GetValidActiveSkills());
        actionPanel.SetSkills(ctx);
        actionPanel.RefreshModifiers(Modifiers);
        actionPanel.ToggleSkillBasedOnMana(Mana);
    }

    [Button]
    private ActionPanel CreateActionPanel()
    {
        var instance = Instantiate(actionPanelPrefab, actionPanelFolder, false);
        ((RectTransform)instance.transform).anchoredPosition3D = Vector3.zero;

        return instance.GetComponent<ActionPanel>();
    }

    private void PayForSkill (Skill skill)
        => Mana -= skill.GetSkillCost();
}