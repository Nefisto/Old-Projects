using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

[Serializable]
public abstract class TraitSector : IEnumerable<Trait>, IEquatable<TraitSector>
{
    protected TraitSector()
    {
        foreach (var gameAttribute in this)
            gameAttribute.OnUpdatedGrow += _ => OnUpdatedAccumulatedPoints?.Invoke();
    }

    [BoxGroup("Bonus", order: -1)]
    [ShowInInspector]
    public virtual int AccumulatedPoints
        => this.Sum(ga => ga.Grow)
           - this.Count(); // Attribute grow starts at 1, but this 1 didn't count through sector accumulated points

    [BoxGroup("Bonus")]
    [ShowInInspector]
    public int PartialPoints => AccumulatedPoints % GameConstants.PARTIAL_ATTRIBUTE_POINTS_AMOUNT;

    [BoxGroup("Bonus")]
    [ShowInInspector]
    public int CompletedPoints => AccumulatedPoints / GameConstants.PARTIAL_ATTRIBUTE_POINTS_AMOUNT;

    public int SumOfTraitsCompletedPoints => this.Sum(t => t.CompletedPoints);
    public int SectorPotential => this.Sum(t => t.Grow);

    public abstract IEnumerator<Trait> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Equals (TraitSector other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return this
            .Zip(other, (trait, otherTrait) => (trait, otherTrait))
            .All(tuple => tuple.trait == tuple.otherTrait);
    }

    public event Action OnUpdatedAccumulatedPoints;

    public void LevelUpTo (int level)
    {
        foreach (var gameAttribute in this)
            gameAttribute.SetPointsToLevel(level, CompletedPoints);
    }

    public void MultiplySector (float multiplyValue)
    {
        foreach (var trait in this)
            trait.MultiplyGrow(multiplyValue);
    }

    public Trait GetGameAttributeFromType (AttributeType type) => this.FirstOrDefault(ga => ga.AttributeType == type);

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((TraitSector)obj);
    }

    // For some reason doing it througth linq sum is breaking
    public override int GetHashCode()
    {
        var hash = 0;
        foreach (var trait in this)
            hash += trait.GetHashCode();

        return hash;
    }

    public static bool operator == (TraitSector left, TraitSector right) => Equals(left, right);

    public static bool operator != (TraitSector left, TraitSector right) => !Equals(left, right);
}