using System;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class GameConstantsSO
{
    [TitleGroup("Skill effect levels")]
    [SerializeField]
    private NDictionary<StatusEffectKind, ChargeAbilityInfo> chargeAbilityKindToInfo;

    [field: TitleGroup("Skill effect levels")]
    [field: SerializeField]
    public NDictionary<AbilityLevel, Color32> ChargeLevelToColor { get; private set; }

    public ChargeAbilityMod GetChargeAbilityMod (StatusEffectKind kind, AbilityLevel level)
        => chargeAbilityKindToInfo[kind]
            .chargeAbilityLevelToInfo[level];

    public ChargeAbilityMod GetChargeAbilityMod (ChargeAbility chargeAbility)
        => chargeAbilityKindToInfo[chargeAbility.IconKind]
            .chargeAbilityLevelToInfo[chargeAbility.AbilityLevel];
}

[Serializable]
public class ChargeAbilityInfo
{
    [Multiline]
    [HideLabel]
    // ReSharper disable once NotAccessedField.Global
    public string description;

    public NDictionary<AbilityLevel, ChargeAbilityMod> chargeAbilityLevelToInfo;
}

[Serializable]
public class ChargeAbilityMod
{
    [HorizontalGroup("Multiplier")]
    public float multiplier;
}