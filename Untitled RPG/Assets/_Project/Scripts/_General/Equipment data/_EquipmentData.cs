using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable Unity.NoNullCoalescing

[Serializable]
public abstract partial class EquipmentData : IEquatable<EquipmentData>, IPotentialProvider
{
    [field: TitleGroup("Settings")]
    [field: HorizontalGroup("Settings/Header", 70)]
    [field: VerticalGroup("Settings/Header/Left")]
    [field: PreviewField(ObjectFieldAlignment.Left)]
    [field: HideLabel]
    [field: SerializeField]
    public Sprite Icon { get; protected set; }

    [field: HorizontalGroup("Settings/Header")]
    [field: VerticalGroup("Settings/Header/Right")]
    [field: PropertyOrder(1)]
    [field: LabelWidth(50)]
    [field: SerializeField]
    public string Name { get; protected set; }

    [field: TitleGroup("Skills", order: 5)]
    [field: SerializeField]
    public Skill DefaultSkill { get; protected set; }

    [field: TitleGroup("Skills")]
    [field: SerializeField]
    public Skill SkillA { get; protected set; }

    [field: TitleGroup("Skills")]
    [field: SerializeField]
    public Skill SkillB { get; protected set; }

    [field: TitleGroup("Trait", order: 3)]
    [field: PropertyOrder(10)]
    [field: HideLabel]
    [field: SerializeField]
    public Potential Potential { get; protected set; }

    [HorizontalGroup("Settings/Header")]
    [VerticalGroup("Settings/Header/Right")]
    [LabelText("Potential")]
    [DisplayAsString(TextAlignment.Right, true, FontSize = 16)]
    [ShowInInspector]
    protected virtual string PotentialLabel
    {
        get => Potential.NicefiedString;
        set => _ = value;
    }

    public abstract EquipmentKind EquipmentKind { get; }

    public bool Equals (EquipmentData other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Name == other.Name;
    }

    public IEnumerable<Skill> SkillIterator()
    {
        yield return (SkillA ? SkillA : Skill.EmptySkill).GetInstance;
        yield return (SkillB ? SkillB : Skill.EmptySkill).GetInstance;
    }
}