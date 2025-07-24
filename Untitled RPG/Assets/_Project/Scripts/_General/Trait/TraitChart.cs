using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public partial class TraitChart : IPotentialProvider, IEquatable<TraitChart>
{
    [field: TabGroup("Attributes", "Strength", true)]
    [field: HideLabel]
    [field: HideReferenceObjectPicker]
    [field: SerializeField]
    public StrengthSector StrengthSector { get; protected set; } = new();

    [field: TabGroup("Attributes", "Vitality")]
    [field: HideLabel]
    [field: HideReferenceObjectPicker]
    [field: SerializeField]
    public VitalitySector VitalitySector { get; protected set; } = new();

    [field: TabGroup("Attributes", "Dexterity")]
    [field: HideLabel]
    [field: HideReferenceObjectPicker]
    [field: SerializeField]
    public DexteritySector DexteritySector { get; protected set; } = new();

    [field: TabGroup("Attributes", "Intelligence")]
    [field: HideLabel]
    [field: HideReferenceObjectPicker]
    [field: SerializeField]
    public IntelligenceSector IntelligenceSector { get; protected set; } = new();

    public TraitChart()
    {
        foreach (var gameAttribute in GameAttributeIterator())
        {
            gameAttribute.OnUpdatedGrow += _ => LevelUpTo(level);
            LevelUpTo(level);
        }

        StrengthSector.OnUpdatedAccumulatedPoints += RaiseOnSpentPoints;
        DexteritySector.OnUpdatedAccumulatedPoints += RaiseOnSpentPoints;
        VitalitySector.OnUpdatedAccumulatedPoints += RaiseOnSpentPoints;
        IntelligenceSector.OnUpdatedAccumulatedPoints += RaiseOnSpentPoints;
    }

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public int SpentPoints
        => StrengthSector.AccumulatedPoints
           + VitalitySector.AccumulatedPoints
           + DexteritySector.AccumulatedPoints
           + IntelligenceSector.AccumulatedPoints;

    private IEnumerable<Trait> GameAttributeIterator()
    {
        foreach (var gameAttribute in StrengthSector)
            yield return gameAttribute;

        foreach (var gameAttribute in DexteritySector)
            yield return gameAttribute;

        foreach (var gameAttribute in VitalitySector)
            yield return gameAttribute;

        foreach (var gameAttribute in IntelligenceSector)
            yield return gameAttribute;
    }

    private IEnumerable<TraitSector> TraitSectorIterator()
    {
        yield return StrengthSector;
        yield return VitalitySector;
        yield return DexteritySector;
        yield return IntelligenceSector;
    }

    private void RaiseOnSpentPoints()
    {
        OnUpdateSpendPoints?.Invoke();
    }

    public event Action OnUpdateSpendPoints;

    [TitleGroup("Debug")]
    [DisableInEditorMode]
    [Button]
    public void LevelUpTo (int level)
    {
        StrengthSector.LevelUpTo(level);
        DexteritySector.LevelUpTo(level);
        VitalitySector.LevelUpTo(level);
        IntelligenceSector.LevelUpTo(level);
    }

    public Trait GetGameAttributeFromType (AttributeType type)
        => StrengthSector.GetGameAttributeFromType(type)
           ?? DexteritySector.GetGameAttributeFromType(type)
           ?? VitalitySector.GetGameAttributeFromType(type)
           ?? IntelligenceSector.GetGameAttributeFromType(type);

    public void MultiplyTraitChart (float multiplier)
    {
        StrengthSector.MultiplySector(multiplier);
        VitalitySector.MultiplySector(multiplier);
        DexteritySector.MultiplySector(multiplier);
        IntelligenceSector.MultiplySector(multiplier);
    }
}