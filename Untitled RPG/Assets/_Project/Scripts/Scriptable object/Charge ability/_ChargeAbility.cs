using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class ChargeAbility : ScriptableObject
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public AbilityLevel AbilityLevel { get; protected set; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    public abstract StatusEffectKind IconKind { get; }

    public virtual void Setup() { }
}