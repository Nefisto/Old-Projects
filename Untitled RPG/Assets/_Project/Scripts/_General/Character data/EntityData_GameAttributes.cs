using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class ActorData
{
    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Other")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public Vector2Int InitialATBPointsRange => new(0, 400);

    // TODO: Add Weapon base damage to calculations
    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Damage")]
    [GUIColor(GameConstants.DEXTERITY_HEX_COLOR)]
    [DisplayAsString(TextAlignment.Right, true)]
    [ShowInInspector]
    public int DexterityDamage => DamageCalculation(TraitChart.DexteritySector, DexterityPotential);

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Damage")]
    [GUIColor(GameConstants.VITALITY_HEX_COLOR)]
    [DisplayAsString(TextAlignment.Right, true)]
    [ShowInInspector]
    public int VitalityDamage => DamageCalculation(TraitChart.VitalitySector, VitalityPotential);

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Damage")]
    [GUIColor(GameConstants.STRENGTH_HEX_COLOR)]
    [DisplayAsString(TextAlignment.Right, true)]
    [ShowInInspector]
    public int StrengthDamage => DamageCalculation(TraitChart.StrengthSector, StrengthPotential);

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Damage")]
    [GUIColor(GameConstants.INTELLIGENCE_HEX_COLOR)]
    [DisplayAsString(TextAlignment.Right, true)]
    [ShowInInspector]
    public int IntelligenceDamage => DamageCalculation(TraitChart.IntelligenceSector, IntelligencePotential);

    // STR
    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Strength")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int HealthRegen
        => 1 + Mathf.RoundToInt((StrengthPotential + TraitChart.StrengthSector.HealthRegenAttribute) * .1f);

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Strength")]
    [DisplayAsString(TextAlignment.Right)]
    [EnableGUI]
    [ShowInInspector]
    public int Accuracy => 4 + (StrengthPotential + TraitChart.StrengthSector.AccuracyAttribute.CompletedPoints) * 6;

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Strength")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int CriticalDamage
        => 50
           + Mathf.RoundToInt((StrengthPotential + TraitChart.StrengthSector.CriticalDamageAttribute.CompletedPoints) * .5f);

    // VIT
    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Vitality")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int HealthMax
        => 30 + (VitalityPotential + TraitChart.VitalitySector.HealthMaxAttribute.CompletedPoints) * 15;

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Vitality")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int Defense => 5 + (VitalityPotential + TraitChart.VitalitySector.DefenseAttribute.CompletedPoints) * 7;

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Vitality")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int Resistance
        => 5 + (VitalityPotential + TraitChart.VitalitySector.ResistanceAttribute.CompletedPoints) * 7;

    // DEX
    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Dexterity")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int Evasion => 5 + (DexterityPotential + TraitChart.DexteritySector.EvasionAttribute) * 4;

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Dexterity")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int CriticalChance
        => DexterityPotential + TraitChart.DexteritySector.CriticalChanceAttribute.CompletedPoints * 2;

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Dexterity")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int AttackSpeed
        => Mathf.RoundToInt((DexterityPotential + TraitChart.DexteritySector.AttackSpeedAttribute.CompletedPoints)
                            * .05f);

    // INT
    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Intelligence")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int ManaMax
        => 20 + (IntelligencePotential + TraitChart.IntelligenceSector.ManaMaxAttribute.CompletedPoints) * 7;

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Intelligence")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int ManaRegen
        => 5 * Mathf.RoundToInt(1 + IntelligencePotential + TraitChart.IntelligenceSector.CompletedPoints);

    [FoldoutGroup("Attributes")]
    [BoxGroup("Attributes/Intelligence")]
    [DisplayAsString(TextAlignment.Right)]
    [ShowInInspector]
    public int Proficiency
        => 3 + (IntelligencePotential + TraitChart.IntelligenceSector.ProficiencyAttribute.CompletedPoints) * 9;

    [FoldoutGroup("Attributes")]
    [ShowInInspector]
    public int RecoveryReduction => 0;

    private static int DamageCalculation (TraitSector traitSector, int entityPotential)
        => Mathf.Max(1, traitSector.SectorPotential)
           + (traitSector.SumOfTraitsCompletedPoints + entityPotential) * traitSector.SectorPotential;

    public IEnumerable<(string attributeName, int attributeValue)> GameAttributesEnumerator()
        => ((IGameAttributes)this).GameAttributeEnumerator();
}