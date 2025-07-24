using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class StrengthSector : TraitSector
{
    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait AccuracyAttribute { get; protected set; } = new(AttributeType.Accuracy);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait HealthRegenAttribute { get; protected set; } = new(AttributeType.HealthRegen);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait CriticalDamageAttribute { get; protected set; } = new(AttributeType.CriticalDamage);

    public override IEnumerator<Trait> GetEnumerator()
    {
        yield return AccuracyAttribute;
        yield return HealthRegenAttribute;
        yield return CriticalDamageAttribute;
    }
}