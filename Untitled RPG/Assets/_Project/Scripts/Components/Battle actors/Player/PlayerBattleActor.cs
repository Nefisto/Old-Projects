using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBattleActor : BattleActor
{
    public static EntryPoint<BattleActionContext> afterCreateBattleActionContextEntryPoint = new();

    [TitleGroup("References")]
    [SerializeField]
    private GradientBar manaBar;

    [FormerlySerializedAs("skillButtonsUI")]
    [TitleGroup("References")]
    [SerializeField]
    private BattleUI battleUI;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    protected Skill selectedSkill;

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: SerializeField]
    public PlayableCharacterData RuntimeData { get; private set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    protected Skill ValidSkill
        => selectedSkill
            ? selectedSkill
            : CurrentWeapon.DefaultSkill;

    public override ActorData ActorData => ServiceLocator.SessionManager.PlayableCharacterData;

    private WeaponData CurrentWeapon => RuntimeData.CurrentEquipment.CurrentWeapon;

    public override int GetATBGrownSpeed
        => Mathf.RoundToInt((GameConstants.MINIMUM_ATB_GROWTH + ActorData.AttackSpeed + CurrentWeapon.WeaponATBPoints)
                            * cachedATBModifier);

    protected void Awake() => GameEvents.onSelectSkill += SetSkill;

    public override IEnumerator SetupBattleStart (SetupBattleActorContext context)
    {
        yield return base.SetupBattleStart(context);

        afterCreateBattleActionContextEntryPoint.Clear();

        RuntimeData = (PlayableCharacterData)context.data.Clone();
        yield return RuntimeData.Job.LoadInstances();

        Currency = RuntimeData.CurrentCurrency;

        var currentWeapon = RuntimeData.CurrentEquipment.CurrentWeapon;
        var currentArmor = RuntimeData.CurrentEquipment.CurrentArmor;
        var allSkills = currentWeapon
            .SkillIterator()
            .Concat(currentArmor.SkillIterator())
            .ToList();

        foreach (var skill in allSkills)
            yield return skill.Setup();

        yield return battleUI.Setup(new BattleUI.SetupSettings()
        {
            defaultAttack = currentWeapon.DefaultSkill,
            defaultDefense = currentArmor.DefaultSkill,
            skills = allSkills,
            casterManaResource = ManaResource,
            job = RuntimeData.Job
        });

        ATBResource.OnReachMax += () =>
        {
            StartCoroutine(RegisterBattleAction(new RegisterBattleActionContext
            {
                onActionCast =
                    () =>
                    {
                        ATBResource.Current = -ValidSkill.FatigueAmount;

                        GameEvents.onSelectSkill.Invoke(CanCastSkill(ValidSkill)
                            ? ValidSkill
                            : CurrentWeapon.DefaultSkill);
                    }
            }));
        };

        SetupPassives();
        RefreshATBModifier();
        GameEvents.onSelectSkill.Invoke(currentWeapon.DefaultSkill);
    }

    public override IEnumerator BattleFinishSetup()
    {
        ServiceLocator.SessionManager.PlayableCharacterData.SetCurrency(Currency);
        yield break;
    }

    public override void Tick()
    {
        if (!canTick)
            return;

        if (StatusEffectController.Any(se => se.ShouldBlockATB))
            return;

        if (Blackboard.CurrentChargingSkill != null)
        {
            var power = ATBResource.TickGrowth;
            Blackboard.CurrentChargingSkill.HoldStay(new HoldSettings { holdPower = power });
        }
        else
            ATBResource.Tick();

        HealthResource.Tick();
        ManaResource.Tick();
    }

    private void SetupPassives()
    {
        foreach (var weaponPassive in CurrentWeapon.GetPassives())
        {
            weaponPassive.Setup(new SetupPassiveSkillContext { owner = this });

            StartCoroutine(weaponPassive.Register());

            GameEvents.OnBattleFinishedEntryPoint += _ => StartCoroutine(weaponPassive.Remove());
        }
    }

    protected override void SetSkill (Skill skill)
    {
        base.SetSkill(skill);

        selectedSkill = skill;
    }

    private IEnumerator RegisterBattleAction (RegisterBattleActionContext ctx)
    {
        var targets = (ServiceLocator.TargetSelector.HasSelectedTarget
            ? new List<BattleActor> { ServiceLocator.TargetSelector.CurrentTarget }
            : ServiceLocator.TargetController.GetTargets(ValidSkill.Target, this)).ToList();

        var battleActionContext = new BattleActionContext(this, ValidSkill, targets)
        {
            OnActionRecoveryFinished = ctx.onActionFinishedRecovery,
            OnActionCast = ctx.onActionCast
        };

        yield return afterCreateBattleActionContextEntryPoint?.YieldableInvoke(battleActionContext);

        ServiceLocator
            .TurnController
            .Add(battleActionContext);
    }

    private bool CanCastSkill (Skill skill)
        => !skill.IsOnCooldown && skill.SyncedResource.Current >= skill.ResourceCost;

    protected override void SetupManaBar()
    {
        base.SetupManaBar();

        manaBar.UpdateBar(ManaResource.CurrentPercentage);
        ManaResource.OnUpdatedCurrent += (_, _) => manaBar.UpdateBar(ManaResource.CurrentPercentage);
    }

    private class RegisterBattleActionContext
    {
        public Action onActionCast;
        public Action onActionFinishedRecovery;
        public Skill skillUsed;
    }
}