using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class TestExtension
{
    public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
    {
        key = tuple.Key;
        value = tuple.Value;
    }
}

/// <summary>
/// This part will control the reference with UI components AND will expose a way to update information
/// </summary>
public partial class ActionPanel : MonoBehaviour
{
    [Title("Control")]
    [field: SerializeField]
    public EventTrigger skipButton { get; private set; }
    
    [Tooltip("Skills' parent")]
    [SerializeField]
    private Transform skillFolder;

    [Tooltip("Modifiers folder")]
    [SerializeField]
    private Transform modifiersFolder;

    [Tooltip("Skill prefab")]
    [SerializeField]
    private GameObject skillPrefab;

    [Tooltip("Modifier prefab")]
    [SerializeField]
    private GameObject modifierPrefab;

    [Tooltip("Used to allow action to register into actor events in awake")]
    [SerializeField]
    private BattleActor owner;
    
    private Dictionary<Skill, Button> skillToButton = new Dictionary<Skill, Button>();

    /// <summary>
    /// Actor will use it to initialize the panel with his skills
    /// </summary>
    /// <param name="context"></param>
    public void SetSkills (UpdateActionPanelContext context)
    {
        foreach (Transform child in skillFolder.transform)
            Destroy(child.gameObject);

        skillToButton.Clear();

        foreach (var skill in context.skills)
        {
            var instance = Instantiate(skillPrefab, skillFolder, true).GetComponent<SkillButton>();
            instance.Setup(skill.icon);

            var button = instance.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                context.actor.ChoseSkill(skill);
                DisablePanel();
            });

            skillToButton.Add(skill, button);
        }
        
        DisablePanel();
    }

    public void ConfigureSkipButton(UpdateActionPanelContext context)
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => context.actor.SkipTurn());
        
        skipButton.triggers.Add(entry);
    }

    public void DisableAllSkills()
    {
        foreach (var (_, button) in skillToButton)
            button.interactable = false;
    }
    
    public void ToggleSkillBasedOnMana (ManaPool manaPool)
    {
        foreach (var (skill, button) in skillToButton)
            button.interactable = manaPool >= skill.GetSkillCost();
    }
    
    /// <summary>
    /// Update the modifiers list
    /// </summary>
    public void RefreshModifiers (List<Modifier> modifiers)
    {
        // Clean modifiers
        foreach (Transform child in modifiersFolder.transform)
        {
            Destroy(child.gameObject);
        }

        // Create modifiers
        foreach (var modifier in modifiers)
        {
            var instance = Instantiate(modifierPrefab, modifiersFolder, true)
                .GetComponent<ModifierIcon>();

            // Modifier should follow the current 
            instance.Setup(modifier.Icon);
        }

        // Correct alpha for modifiers
        ApplyPanelAlphaState();
    }

    public void ShowPanel()
    {
        SetPanelAlphaState(ActionPanelState.Selected);
        
        EnableSkipButton();
    }

    public void DisablePanel()
    {
        SetPanelAlphaState(ActionPanelState.Unselected);

        foreach (var (_, button) in skillToButton)
            button.interactable = false;

        DisableSkipButton();
    }


    public void EnableSkipButton()
        => skipButton.enabled = true;
    
    public void DisableSkipButton()
        => skipButton.enabled = false;

    public void SetOwner (UpdateActionPanelContext ctx)
        => owner = ctx.actor;
}