using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;


public class RunSkillContext
{
    public Skill skill;
}

/// <summary>
/// Our runtime side, its base for friendly and enemies
/// </summary>
[DisallowMultipleComponent] // Can have only one actor
[RequireComponent(typeof(EventTrigger))]
public abstract partial class BattleActor : LazyBehavior, ITargetable
{
    public event Action<StartTurnContext> OnStartTurn;
    public event Action<StartTurnContext> OnFinishStartTurn;
    public event Action<EndTurnContext> OnEndTurn;
    public event Action<RunSkillContext> OnRunSkill;

    [TabGroup("General", true)]
    [Title("Settings")]
    [SerializeField]
    protected ActorData templateData;

    [TabGroup("General")]
    [SerializeField]
    protected BattleActorSide originalSide;

    [TabGroup("General")]
    [ReadOnly]
    [SerializeField]
    private int currentLife;

    [TabGroup("General")]
    [SerializeField]
    protected List<Skill> skills;
    
    [TabGroup("General")]
    [InfoBox("Set some data here for DEBUG reasons only. It should be null for other purposes")]
    [SerializeField]
    private ActorData runtimeData;
    
    protected List<Modifier> Modifiers = new List<Modifier>();

    protected BattleActionContext turnContext;

    /// <summary>
    /// Its useful to filter targets and compare their friends. Also used as reference point when applying modifiers 
    /// </summary>
    public BattleActorSide OriginalSide => originalSide;

    [ShowInInspector]
    public int CurrentLevel => Data != null
        ? Data.Level
        : 0;

    // Lazy approach to data
    public ActorData Data
    {
        get
        {
            if (templateData == null)
                return null;

            if (runtimeData == null)
                runtimeData = (ActorData)templateData.Clone();

            return runtimeData;
        }
    }

    public int CurrentLife => currentLife;

    protected virtual void Awake()
    {
        originalColor = spriteRenderer.color;
        
        SetupTargetableEntry();
    }

    protected void Setup (ActorData data)
    {
        templateData = data;
        currentLife = Data != null ? Data.GetBaseStatus().MaxHealth : 0;
        
        ResetMana();
        SetupInventory();
    }

    public virtual IEnumerator BattleStart()
    {
        var message = RichTextUtility.GetRichTextWithColor(name, originalSide == BattleActorSide.Player ? Color.green : Color.red)
                      + " is ready to battle!";
        GameEvents.Battle.RaiseCombatLogAction(new CustomCombatLog(message));

        yield return new WaitForSeconds(.2f);
    }

    public virtual IEnumerator StartTurn()
    {
        OnStartTurn?.Invoke(new StartTurnContext()
        {
            Actor = this,
            InventoryData = inventory
        });
        
        GameEvents.Battle.RaiseCombatLogAction(new StartTurnCombatLog(this));

        yield return RandomizeMana();
    }

    public virtual IEnumerator RunTurn()
    {
        turnContext = new BattleActionContext()
        {
            caster = this
        };
        
        yield return null;
    }

    public virtual IEnumerator EndTurn()
    {
        GameEvents.Battle.RaiseCombatLogAction(new EndTurnCombatLog(this));
        OnEndTurn?.Invoke(new EndTurnContext());

        yield return new WaitForSeconds(.5f);
    }

    public virtual IEnumerator RunSkill (Skill skill)
    {
        GameEvents.Battle.RaiseCombatLogAction(new CastSkillCombatLog(this, skill));

        yield return new WaitForSeconds(.5f);
        
        // Apply modifiers (Buffs/debuffs) on context

        // Run skill
        yield return skill.Act(turnContext);

        // Check for edge cases
        if (turnContext.targets.Count == 0)
            Debug.LogWarning($"Dont have any valid target.\nAction: {skill.name}\nCaster: {turnContext.caster.name}\n", this);
    }

    public virtual IEnumerator TakeDamage (BattleActionContext context)
    {
        var status = Data.GetCurrentStatus();
        
        var amountResisted = CalculateAmountResisted(context, status);
        var actualDamage = CalculateActualDamage(context, amountResisted);

        // Apply damage
        
        // Raise lo
        GameEvents.Battle.RaiseCombatLogAction(new TakeDamageCombatLog(this, actualDamage, amountResisted));
        
        // Run animation
        yield return BlinkFeedback();

#if !UNITY_EDITOR
        yield return Die();
#endif

        // Trigger events
    }

    private int CalculateActualDamage(BattleActionContext context, int amountResisted)
        => context.actionValue - amountResisted;

    private int CalculateAmountResisted(BattleActionContext context, Status status)
        => status.Intelligence >= context.actionValue ? context.actionValue : status.Intelligence;

    public virtual void Heal (BattleActionContext context)
    {
        var amountAmplified = context.actionValue - context.originalActionValue;

        BlinkFeedback();

        GameEvents.Battle.RaiseCombatLogAction(new HealCombatLog(this, context.actionValue, amountAmplified));
    }

    public virtual IEnumerator Die()
    {
        yield return null;
    }

    // TODO: Change the way that the whole modifier attribute was applied
    public virtual void ApplyModifier (Modifier modifier)
    {
        // Already have this modifier applied
        var duplicateModifier = Modifiers.FirstOrDefault(m => m.Equals(modifier));
        if (duplicateModifier != null)
        {
            duplicateModifier.RefreshModifier();

            return;
        }

        // Store modifier
        Modifiers.Add(modifier);

        // Apply modifier
        modifier.Apply(new ModifierContext() { target = this });
    }

    protected virtual IEnumerator SetTargets()
    {
        turnContext.targets = BattleManager.Instance.GetTargets(turnContext);

        yield return null;
    }

    protected virtual void RaiseFinishStartTurn(StartTurnContext ctx)
        => OnFinishStartTurn?.Invoke(ctx);

    protected virtual void RaiseRunSkill (RunSkillContext obj)
    {
        OnRunSkill?.Invoke(obj);
    }
}