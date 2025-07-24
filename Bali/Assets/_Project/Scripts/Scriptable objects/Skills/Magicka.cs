using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class Magicka : SerializedScriptableObject
{
    [Title("Settings")]
    [OdinSerialize]
    public string Name { get; protected set; }
    
    [OdinSerialize]
    [MultiLineProperty(5)]
    public string Description { get; protected set; }

    [OdinSerialize]
    public int BonusDamage { get; protected set; }

    [OdinSerialize]
    public int BonusLife { get; protected set; }

    [Title("Cost")]
    [OdinSerialize]
    [HideLabel]
    [HideReferenceObjectPicker]
    public MagickaResource Cost { get; protected set; } = new();
}