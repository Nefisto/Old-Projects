using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BattleActor : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    protected GradientBar healthBar;

    /// <summary>
    ///     Initially used to disable tick on ONE specific scenario (recovering from skills), if this scale remove it in favor
    ///     of an state machine to disable tick when necessary
    /// </summary>
    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    protected bool canTick = true;

    [field: TitleGroup("References")]
    [field: SerializeField]
    public AnimationController AnimationController { get; private set; }

    [field: TitleGroup("Resource")]
    [field: HideReferenceObjectPicker]
    [field: SerializeField]
    public HealthResource HealthResource { get; protected set; } = new();

    public Action onLeaveCombat;
    public Action<OnDieContext> onDie;

    [field: TitleGroup("Resource")]
    [field: SerializeField]
    public TickResource ATBResource { get; protected set; } = new();

    [field: TitleGroup("Resource")]
    [field: SerializeField]
    public virtual int Currency { get; set; }

    [field: TitleGroup("Resource")]
    [field: SerializeField]
    public ManaResource ManaResource { get; protected set; } = new();

    [field: TitleGroup("References")]
    [field: SerializeField]
    public StatusEffectController StatusEffectController { get; protected set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public ActorATBIcon actorATBIcon;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    protected float cachedATBModifier = 1f;

    public abstract ActorData ActorData { get; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    public BattleActorSide BattleActorSide
        => this is PlayerBattleActor ? BattleActorSide.Player : BattleActorSide.Enemy;

    [TitleGroup("Debug")]
    [ShowInInspector]
    public bool IsOnFatigue => ATBResource.Current < 0;

    public Action<TickResource> CustomResetATB { protected get; set; }

    public virtual int GetATBGrownSpeed
        => Mathf.RoundToInt((GameConstants.MINIMUM_ATB_GROWTH + ActorData.AttackSpeed) * cachedATBModifier);

    public virtual int GetInitialATBPoints
        => Mathf.Clamp(Random.Range(ActorData.InitialATBPointsRange.x, ActorData.InitialATBPointsRange.y + 1),
            GameConstants.DEFAULT_MIN_ATB, GameConstants.DEFAULT_MAX_ATB);

    public virtual IEnumerator SetupBattleStart ([HideLabel] SetupBattleActorContext context)
    {
        StatusEffectController ??= GetComponent<StatusEffectController>();
        StatusEffectController.Setup(this);
        StatusEffectController.OnAddedEffect += RefreshATBModifier;
        StatusEffectController.OnRemovedEffect += RefreshATBModifier;

        HealthResource.Setup(new HealthResource.HealthResourceSetupSettings
        {
            initialMax = ActorData.HealthMax,
            initialPercentage = context.initialHealthPercentage,
            regenerationPerSecond = ActorData.HealthRegen,
            owner = this
        });

        ManaResource.Setup(new ManaResource.ManaResourceSetupSettings
        {
            initialMax = ActorData.ManaMax,
            initialPercentage = context.initialManaPercentage,
            regenerationPerSecond = ActorData.ManaRegen,
            owner = this
        });

        ATBResource.Setup(new TickResource.TickResourceSetupSettings
        {
            initialMax = GameConstants.DEFAULT_MAX_ATB,
            ATBGrownth = () => GetATBGrownSpeed,
            initialPercentage = (float)GetInitialATBPoints / GameConstants.DEFAULT_MAX_ATB
        });

        SetupHealthBar();
        SetupManaBar();
        yield break;
    }

    protected virtual void RefreshATBModifier() => cachedATBModifier = 1 * (1 + StatusEffectController.GetATBModifiers);

    public virtual IEnumerator BattleFinishSetup()
    {
        yield break;
    }

    protected virtual void SetSkill (Skill _) { }

    [TitleGroup("Debug")]
    [Button]
    [DisableInEditorMode]
    public virtual void Tick()
    {
        if (!canTick)
            return;

        if (StatusEffectController.Any(se => se.ShouldBlockATB))
            return;

        ATBResource.Tick();
        HealthResource.Tick();
        ManaResource.Tick();
    }

    [TitleGroup("Debug")]
    [Button]
    [DisableInEditorMode]
    public IEnumerator TakePhysicalDamage (ActionInfo actionInfo)
    {
        if (actionInfo.caster == null)
        {
            ServiceLocator.FloatText.DamageText($"{actionInfo.FinalDamage}", transform);
            yield return InternalTakeDamage(actionInfo.flatDamage);
            yield break;
        }

        if (actionInfo.hasMissed)
        {
            ServiceLocator.FloatText.MissText(transform);
            yield break;
        }

        if (actionInfo.hadCritical)
            ServiceLocator.FloatText.CriticalText(transform);

        ServiceLocator.FloatText.DamageText($"{actionInfo.FinalDamage}", transform);
        yield return InternalTakeDamage(actionInfo.FinalDamage);
    }

    [TitleGroup("Debug")]
    [Button]
    [DisableInEditorMode]
    public IEnumerator HealHealth (ActionInfo actionInfo)
    {
        HealthResource.Current += actionInfo.FinalHealing;

        ServiceLocator.FloatText.AddCustomFloatText(new FloatTextSettings($"{actionInfo.FinalHealing}", transform,
            textColor: Color.green));

        yield return null;
    }

    [TitleGroup("Debug")]
    [DisableInEditorButton]
    public IEnumerator TakePoisonDamage (int damage)
    {
        ServiceLocator.FloatText.PoisonText($"{damage}", transform);
        yield return InternalTakeDamage(damage);
    }

    [TitleGroup("Debug")]
    [DisableInEditorButton]
    public IEnumerator TakeBleedDamage (int damage)
    {
        ServiceLocator.FloatText.BleedText($"{damage}", transform);
        yield return InternalTakeDamage(damage);
    }

    private IEnumerator InternalTakeDamage (int damage)
    {
        HealthResource.Current -= damage;
        yield return AnimationController.TakeDamageAnimation();

        if (HealthResource.Current <= 0)
            yield return Die();
    }

    public IEnumerator HealMana (int amount)
    {
        ManaResource.Current += amount;

        ServiceLocator.FloatText.AddCustomFloatText(
            new FloatTextSettings($"{amount}", transform, textColor: Color.blue));

        yield return null;
    }


    protected virtual IEnumerator Die()
    {
        onDie?.Invoke(new OnDieContext());
        yield break;
    }

    protected virtual void SetupManaBar() => ManaResource.Reset();

    protected virtual void SetupHealthBar()
    {
        healthBar.UpdateBar(HealthResource.CurrentPercentage);
        HealthResource.OnUpdatedCurrent += (_, _) => healthBar.UpdateBar(HealthResource.CurrentPercentage);
    }

    public virtual int AmountOfBuffOrDebuffs (StatusEffectData.BuffOrDebuff buffOrDebuff)
        => StatusEffectController.AmountOf(buffOrDebuff);

    public bool IsActorDead() => HealthResource.Current <= 0;
}