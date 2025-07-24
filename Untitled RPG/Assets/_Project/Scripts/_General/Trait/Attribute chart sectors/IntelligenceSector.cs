using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class IntelligenceSector : TraitSector
{
    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait ProficiencyAttribute { get; protected set; } = new(AttributeType.Proficiency);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait ManaMaxAttribute { get; protected set; } = new(AttributeType.ManaMax);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait ManaRegenAttribute { get; protected set; } = new(AttributeType.ManaRegen);

    public override IEnumerator<Trait> GetEnumerator()
    {
        yield return ProficiencyAttribute;
        yield return ManaMaxAttribute;
        yield return ManaRegenAttribute;
    }
}