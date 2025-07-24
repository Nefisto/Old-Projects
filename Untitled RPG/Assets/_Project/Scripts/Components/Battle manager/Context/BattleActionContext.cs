using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable NotAccessedField.Global

[Serializable]
public class BattleActionContext : IEnumerable<ActionInfo>
{
    [HideInInspector]
    public BattleActor caster;

    [HideInInspector]
    public Skill skill;

    public Action OnActionCast;
    public Action OnActionRecoveryFinished;

    [field: SerializeField]
    [field: ListDrawerSettings(ShowFoldout = true, ListElementLabelName = nameof(ActionInfo.targetName))]
    public List<ActionInfo> Actions { get; set; } = new();

    [HideLabel]
    [DisplayAsString(TextAlignment.Left, true)]
    [PropertyOrder(-10)]
    [ShowInInspector]
    private string action = "EMPTY";

    public BattleActionContext (BattleActor caster, Skill skill, List<BattleActor> targets)
    {
        this.caster = caster;
        this.skill = skill;

        Actions = targets
            .Select(target => new ActionInfo
            {
                caster = caster,
                target = target,
                skill = skill,
                targetName = target.ActorData.Name
            })
            .ToList();

        action = $"<b>{caster.ActorData.Name}</b> used <b>{skill.name}</b> on:";
    }

    public IEnumerator<ActionInfo> GetEnumerator() => Actions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void ChangeTargets (List<BattleActor> newTargets)
    {
        Actions = newTargets
            .Select(target => new ActionInfo
            {
                caster = caster,
                target = target,
                targetName = target.ActorData.Name
            })
            .ToList();
    }
}

[Serializable]
public class CustomPassive
{
    [HideLabel]
    [HorizontalGroup]
    [ShowInInspector]
    public string passiveName;

    [HideLabel]
    [HorizontalGroup]
    [ShowInInspector]
    public string passiveInfo;

    public Action<ActionInfo> actionOnInfo;
}

public class PassiveInfo
{
    [HideLabel]
    [HorizontalGroup(.6f)]
    [DisplayAsString]
    public string passiveName;

    public PassiveInfo (string passiveName) => this.passiveName = passiveName;
}

[Serializable]
public class DamageModifierPassiveInfo : PassiveInfo
{
    [HideLabel]
    [HorizontalGroup]
    [DisplayAsString]
    public float value;

    public DamageModifierPassiveInfo (string passiveName, float value) : base(passiveName) => this.value = value;
}

[Serializable]
public class EffectInfo
{
    [Tooltip("Has been casted?")]
    [HideLabel]
    [HorizontalGroup(.08f)]
    public bool hasBeenCasted = true;

    [Tooltip("Has been applied?")]
    [HideLabel]
    [HorizontalGroup(.08f)]
    public bool? hasApplied;

    // TODO: later this need to be based on status effect base chance
    [Tooltip("Base chance to apply")]
    [HideLabel]
    [DisplayAsString]
    [HorizontalGroup(.12f)]
    public float baseChanceToApplyEffect;

    [HideLabel]
    [HorizontalGroup]
    public StatusEffectData data;

    public Action onSuccessfullyApplied;
    public Action onFailureApplied;
}