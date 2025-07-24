using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DexteritySector : TraitSector
{
    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait EvasionAttribute { get; protected set; } = new(AttributeType.Evasion);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait CriticalChanceAttribute { get; protected set; } = new(AttributeType.CriticalChance);

    [PropertyOrder(1)]
    [InlineIndentedPropertyWithTitle]
    [ShowInInspector]
    [field: HideInInspector]
    [field: SerializeField]
    public Trait AttackSpeedAttribute { get; protected set; } = new(AttributeType.AttackSpeed);

    public override IEnumerator<Trait> GetEnumerator()
    {
        yield return EvasionAttribute;
        yield return CriticalChanceAttribute;
        yield return AttackSpeedAttribute;
    }
}