using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

// ReSharper disable Unity.RedundantSerializeFieldAttribute


[Serializable]
public abstract partial class ActorData : IGameAttributes, IPotentialProvider, ICloneable
{
    [HideInInspector]
    [SerializeField]
    private int currentLevel = 1;

    [field: TitleGroup("Settings")]
    [field: HorizontalGroup("Settings/Info")]
    [field: VerticalGroup("Settings/Info/Right")]
    [field: LabelWidth(50)]
    [field: SerializeField]
    public string Name { get; set; }

    [field: TitleGroup("Settings")]
    [field: PropertyOrder(5)]
    [field: SerializeField]
    public TraitChart TraitChart { get; set; }

    public abstract Sprite ATBIcon { get; protected set; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    public int CurrentLevel
    {
        get => Mathf.Max(currentLevel, 1);
        set => currentLevel = Mathf.Max(value, 1);
    }

    public abstract MainTraitKind MainTraitKind { get; }

    public object Clone()
    {
        var instance = (ActorData)SerializationUtility.CreateCopy(this);
        instance.TraitChart = (TraitChart)SerializationUtility.CreateCopy(TraitChart);

        return instance;
    }

    public int Damage
        => MainTraitKind switch
        {
            MainTraitKind.Strength => StrengthDamage,
            MainTraitKind.Vitality => VitalityDamage,
            MainTraitKind.Intelligence => IntelligenceDamage,
            MainTraitKind.Dexterity => DexterityDamage,
            _ => throw new ArgumentOutOfRangeException()
        };

    public virtual IEnumerator Setup (EntityDataSetupContext ctx)
    {
        yield break;
    }

    [TitleGroup("Debug")]
    [DisableInEditorButton]
    public virtual void LevelUpTo (int level)
    {
        CurrentLevel = level;
        TraitChart.LevelUpTo(level);
    }

    public class EntityDataSetupContext
    {
        public BattleActor runtimeActor;
    }
}