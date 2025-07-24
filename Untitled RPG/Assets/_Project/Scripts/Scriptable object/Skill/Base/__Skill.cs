using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts._General.Enum;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Flags]
public enum SkillType
{
    None = 0,
    Damage = 1 << 0,
    Heal = 1 << 1
}

public enum SkillAttribute
{
    ManaCost,
    Cooldown,
    RecoveryTime,
    ChanceToHit,
    CriticalChance
}

public enum MainAttributeOrder
{
    TypicalDamage,
    JustApplyStatus
}

public class HoldSettings
{
    public int holdPower;
}

public class HoldFinishSettings
{
    public bool shouldHideBar = true;
    public bool shouldResetPoints = true;
}

/// <summary>
///     Enemies ATB speed is based on current selected skill
/// </summary>
public enum SkillATBModifier
{
    Slow,
    Normal,
    Fast
}

[Serializable]
public class PlayAnimationSettings
{
    public float animationSpeed = 1f;
    public bool flipX;

    public bool shouldFlipYForPlayer;

    public float offsetX;
    public float offsetY;
    public float size = 1f;
}

public abstract partial class Skill : ScriptableObject
{
    private static Skill emptySkill;

    [field: TabGroup("Tab", "Common")]
    [field: TitleGroup("Tab/Common/Settings")]
    [field: HorizontalGroup("Tab/Common/Settings/Info")]
    [field: SerializeField]
    public string Name { private set; get; }

    [field: HorizontalGroup("Tab/Common/Settings/Info")]
    [field: HideLabel]
    [field: SerializeField]
    [field: PreviewField]
    public Sprite Icon { get; protected set; }

    [field: TitleGroup("Tab/Common/Settings")]
    [field: SerializeField]
    [field: Multiline(5)]
    public string Description { private set; get; }

    [field: TitleGroup("Tab/Common/Settings")]
    [field: Range(0, GameConstants.MAX_FATIGUE_AMOUNT)]
    [field: SerializeField]
    public int FatigueAmount { get; private set; }

    [field: TitleGroup("Tab/Common/Settings")]
    [field: SerializeField]
    public float Cooldown { get; private set; }

    [field: TitleGroup("Tab/Common/Settings")]
    [field: SerializeField]
    public int ResourceCost { get; private set; }

    [field: TitleGroup("Tab/Common/Settings")]
    [field:
        Tooltip(
            "This guy will be responsible to tell to skill which are the important attributes to showcase first on cases like HUD")]
    [field: SerializeField]
    public MainAttributeOrder MainAttributesOrder { get; private set; }

    [field: TitleGroup("Tab/Common/Settings")]
    [field: SerializeField]
    public SkillType SkillType { get; private set; }

    [Space]
    [TitleGroup("Tab/Common/Settings")]
    [SerializeField]
    protected List<StatusEffectToApply> effectToApply;

    [Tooltip("Should it pause the battle during animation?")]
    [SerializeField]
    [HideInInspector]
    private bool shouldWaitForFinish;

    [field: TitleGroup("Tab/Common/Settings")]
    [field: SerializeField]
    public bool AlwaysHit { get; private set; }

    [field: TitleGroup("Tab/Common/Settings")]
    [field: HideIf("@AlwaysHit == true")]
    [field: Range(.1f, .95f)]
    [field: SerializeField]
    public float ChanceToHit { get; protected set; } = .95f;

    [field: TitleGroup("Tab/Common/Settings")]
    [field: Range(0f, 1f)]
    [field: SerializeField]
    public float CriticalChance { get; protected set; } = .05f;

    [field: TitleGroup("Tab/Common/Settings")]
    [field: SerializeField]
    public GroupTarget Target { get; protected set; }

    [TitleGroup("Tab/Common/Settings")]
    [SerializeField]
    private SkillATBModifier atbModifier = SkillATBModifier.Normal;

    [field: TabGroup("Tab", "Charge")]
    [field: TitleGroup("Tab/Charge/Settings")]
    [field: SerializeField]
    public List<ChargeLevelSettings> ChargePointsSettings { get; set; }

    public EntryPoint<BattleActionContext> OnFinishedAnimationEntryPoint = new();
    public EntryPoint<BattleActionContext> OnBeginningAnimationEntryPoint = new();

    public EntryPoint<BattleActionContext> GettingActionInfoEntryPoint { get; set; } = new();

    public Skill GetInstance => Instantiate(this);

    [TitleGroup("Debug")]
    [Tooltip("Should reset the target on battle when this skill is selected?")]
    [ShowInInspector]
    public bool ShouldResetTarget => SkillType.HasFlag(SkillType.Heal);

    [field: TitleGroup("Debug", order: 10)]
    [field: ReadOnly]
    [field: ShowInInspector]
    public bool IsOnCooldown { get; set; }

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: ShowInInspector]
    public IGameResource SyncedResource { get; set; }

    public static Skill EmptySkill => emptySkill ? emptySkill : emptySkill = CreateInstance<DoNothingSkill>();

    public int SkillATBModifierBonus
        => atbModifier switch
        {
            SkillATBModifier.Slow => 0,
            SkillATBModifier.Normal => 2,
            SkillATBModifier.Fast => 4,
            _ => throw new ArgumentOutOfRangeException()
        };

    public event Action OnCast;

    public List<BattleActor> GetSkillTargets (BattleActor caster)
        => ServiceLocator
            .TargetController
            .GetTargets(Target, caster)
            .ToList();

    public virtual IEnumerator Setup()
    {
        IsOnCooldown = false;
        foreach (var chargePointsSetting in ChargePointsSettings)
            chargePointsSetting.chargeAbilities = chargePointsSetting
                .chargeAbilities
                .Select(ca => ca.GetInstance())
                .ForEach(ca => ca.Setup())
                .ToList();
        yield break;
    }

    public virtual IEnumerator Run (BattleActionContext context)
    {
        var battleLogMessage = ServiceLocator.BattleLogPooler.GetPooledObject(true);
        battleLogMessage.Setup(Name);

        yield return BeforeCalculateActionValues(context);
        yield return CalculateActionValues(context);
        foreach (var actionInfo in context)
            yield return AfterCalculateActionValues(actionInfo);

        if (animationTypeEnum != SkillAnimationType.None)
            yield return RunAnimation(context);

        GameEvents.onRunBattleAction?.Invoke(context);

        yield return Behavior(context);
        OnCast?.Invoke();

        yield return ApplyEffect(context);

        ServiceLocator.BattleLogPooler.ReturnPooledObject(battleLogMessage);
    }

    protected virtual IEnumerator BeforeCalculateActionValues (BattleActionContext context)
    {
        yield break;
    }

    protected virtual IEnumerator Behavior (BattleActionContext context)
    {
        yield return new WaitForSeconds(.2f);

        foreach (var actionInfo in context)
            yield return actionInfo.target.TakePhysicalDamage(actionInfo);

        yield return HoldFinish(new HoldFinishSettings { shouldHideBar = true });
    }

    private IEnumerator CalculateActionValues (BattleActionContext context)
    {
        yield return GettingActionInfoEntryPoint?.YieldableInvoke(context);
        GettingActionInfoEntryPoint?.Clear();

        foreach (var action in context)
            yield return CalculateActionValues(action);
    }

    private IEnumerator CalculateActionValues (ActionInfo info)
    {
        var attackerData = info.caster.ActorData;
        var defenderData = info.target.ActorData;

        info.baseCritical = CriticalChance;
        info.criticalBonus.Add(("Attribute", attackerData.CriticalChance / 100f));
        info.criticalChance = info.baseCritical * (1 + info.criticalBonus.Sum(t => t.Item2));
        info.criticalAttribute = attackerData.CriticalChance;
        info.hadCritical = Random.value < info.criticalChance;
        info.criticalDamagePercentage = attackerData.CriticalDamage / 100f;

        info.evasionAttribute = defenderData.Evasion;
        info.evasionBonus = BattleFormulas.GetEvadeBonus(defenderData.Evasion, attackerData.CurrentLevel);
        info.accuracyAttribute = attackerData.Accuracy;
        info.hitBonus = BattleFormulas.GetHitBonus(attackerData.Accuracy, defenderData.CurrentLevel);
        if (AlwaysHit)
        {
            info.chanceToHit = 1f;
        }
        else
        {
            info.chanceToHit = BattleFormulas.GetSkillChanceToHit(ChanceToHit, info.hitBonus, info.evasionBonus);
            info.chanceToHit = Mathf.Clamp(info.chanceToHit + info.flatBonusToHit.Sum(x => x.Item2), 0f, 1f);
            info.hasMissed = Random.value > info.chanceToHit;
        }

        info.flatDamage = GetDamage(attackerData);
        info.multipliedDamage =
            Mathf.RoundToInt(info.flatDamage * (1 + info.percentageOfFlatDamageBonus.Sum(t => t.percentage)));
        info.damageAfterCritical =
            BattleFormulas.ApplyCriticalDamage(info.multipliedDamage, attackerData.CriticalDamage);
        info.targetDefense = defenderData.Defense;
        info.mitigatedPercentage = BattleFormulas.GetDamageReduction(defenderData.Defense, attackerData.CurrentLevel);
        info.mitigatedDamage = Mathf.RoundToInt(info.BaseDamage * info.mitigatedPercentage);

        info.proficiencyBonus = BattleFormulas.GetProficiencyBonus(attackerData.Proficiency, defenderData.CurrentLevel);
        info.resistanceBonus = BattleFormulas.GetResistanceBonus(defenderData.Resistance, attackerData.CurrentLevel);

        foreach (var infoCustomPassive in info.customPassives)
            infoCustomPassive.actionOnInfo?.Invoke(info);
        yield break;
    }

    protected virtual IEnumerator AfterCalculateActionValues (ActionInfo info)
    {
        foreach (var effect in effectToApply)
            info.effectInfo.Add(new EffectInfo
            {
                data = effect.statusEffect,
                hasBeenCasted = Random.value <= effect.baseChanceToTrigger,
                baseChanceToApplyEffect = effect.baseChanceToApply
            });

        yield break;
    }

    protected virtual int GetDamage (ActorData data) => data.Damage;
    
    protected virtual IEnumerator RunAnimation (BattleActionContext context)
    {
        var amountOfTargets = context.Actions.Count;
        var anim = ServiceLocator.SkillVFX.GetMultipleSkillAnimator(amountOfTargets);

        yield return OnBeginningAnimationEntryPoint?.YieldableInvoke(context);
        
        var routines = new List<NTask>();
        for (var i = 0; i < amountOfTargets; i++)
        {
            var currentAnim = anim[i];
            currentAnim.transform.position = context.Actions[i].target.transform.position;

            var task = new NTask(currentAnim.Play(animationTypeEnum.ToString(),
                GetAnimationSettings(context.caster is EnemyBattleActor)));

            task.OnFinished += _ =>
            {
                ServiceLocator.SkillVFX.ReturnSkillAnimator(currentAnim);
                routines.Remove(task);
            };

            routines.Add(task);
        }

        yield return ShouldWaitForFinish
            ? new WaitUntil(() => routines.Count == 0)
            : null;

        yield return OnFinishedAnimationEntryPoint?.YieldableInvoke(context);
    }

    // This will wait for steps like begin animation BUT will apply on all enemies at same time
    // i.e. a poison all should start the begin animation at same target when running on multiple enemies BUT it should
    //      block the main battle routine while ALL animations are running
    protected IEnumerator ApplyEffect (BattleActionContext context)
    {
        var runningEffects = new List<NTask>();
        foreach (var actionInfo in context)
        {
            foreach (var effectInfo in actionInfo.effectInfo.Where(info => info.hasBeenCasted))
            {
                // Just try to resist for debuffs
                if (effectInfo.data.ConditionState == StatusEffectData.BuffOrDebuff.Debuff)
                {
                    if (!effectInfo.hasApplied.HasValue)
                    {
                        var chanceToApply = BattleFormulas.GetEffectChanceToApply(effectInfo.baseChanceToApplyEffect,
                            actionInfo.proficiencyBonus, actionInfo.resistanceBonus);
                        effectInfo.hasApplied = Random.value <= chanceToApply;
                    }

                    if (!effectInfo.hasApplied.Value)
                    {
                        effectInfo.onFailureApplied?.Invoke();

                        ServiceLocator.FloatText.AddCustomFloatText(new FloatTextSettings("resisted!",
                            actionInfo.target.transform, textColor: Color.cyan));
                        continue;
                    }
                }

                effectInfo.onSuccessfullyApplied?.Invoke();
                var target = effectInfo.data.EffectTarget == StatusEffectData.StatusEffectTarget.Target
                    ? actionInfo.target
                    : actionInfo.caster;
                var newTask = new NTask(target.StatusEffectController.ApplyStatusEffect(
                        new List<StatusEffectData> { effectInfo.data }),
                    false);

                runningEffects.Add(newTask);
                newTask.OnFinished += _ =>
                {
                    runningEffects.Remove(newTask);

                    GameEvents.OnPause -= newTask.Pause;
                    GameEvents.OnUnpause -= newTask.Unpause;
                };

                GameEvents.OnPause += newTask.Pause;
                GameEvents.OnUnpause += newTask.Unpause;
            }
        }

        foreach (var runningEffect in runningEffects)
            runningEffect.Start();

        yield return new WaitUntil(() => runningEffects.IsEmpty());
    }

    [Serializable]
    public class StatusEffectToApply
    {
        [HorizontalGroup]
        [HideLabel]
        public StatusEffectData statusEffect;

        [Range(0f, 1f)]
        public float baseChanceToTrigger;

        [Range(0f, 2f)]
        public float baseChanceToApply = .5f;
    }

    [Serializable]
    public class ChargeLevelSettings
    {
        public int pointsToLevelUp;

        public List<ChargeAbility> chargeAbilities;
    }
}