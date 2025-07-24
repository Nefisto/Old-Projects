using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class VitalitySector : TraitSector
{
    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait HealthMaxAttribute { get; protected set; } = new(AttributeType.HealthMax);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait DefenseAttribute { get; protected set; } = new(AttributeType.Defense);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait ResistanceAttribute { get; protected set; } = new(AttributeType.Resistance);

    public override IEnumerator<Trait> GetEnumerator()
    {
        yield return HealthMaxAttribute;
        yield return DefenseAttribute;
        yield return ResistanceAttribute;
    }
}