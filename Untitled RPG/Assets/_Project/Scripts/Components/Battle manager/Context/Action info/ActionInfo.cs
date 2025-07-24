using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public partial class ActionInfo
{
    [HideInInspector]
    public BattleActor caster;

    [HideInInspector]
    public BattleActor target;

    [HideInInspector]
    public Skill skill;

    [HideInInspector]
    public string targetName = "Empty";

    [BoxGroup("Custom passives")]
    [ShowInInspector]
    public List<CustomPassive> customPassives = new();

    [TabGroup("Passives applied")]
    [HideReferenceObjectPicker]
    public List<PassiveInfo> otherPassives = new();
}