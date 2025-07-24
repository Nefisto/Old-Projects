using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Status : IComparable<Status>
{
    [PropertyOrder(-2)]
    [MinValue(1)]
    public int level = 1;

    [ReadOnly]
    [PropertyOrder(-1)]
    public int experience = 0;

    [PropertySpace]
    [Title("Attributes")]
    [ShowInInspector]
    [PropertyOrder(-1)]
    public int MaxHealth => attributes.health + HealthFromLevel;

    public int Strength => attributes.strength;
    public int Dexterity => attributes.dexterity;
    public int Intelligence => attributes.intelligence;

    [HideLabel]
    [SerializeField]
    private CharacterAttributes attributes = new CharacterAttributes()
    {
        health = 0,
        strength = 100,
        dexterity = 100,
        intelligence = 100
    };

    public static Status EmptyStatus => new Status
    {
        level = 0,
        attributes = new CharacterAttributes()
    };

    private int HealthFromLevel => Mathf.Clamp(100 + ((level - 1) * 50), 100, 9999);

    public static Status ConstructWithAttributes (CharacterAttributes attributes)
        => new Status
        {
            level = 0,
            attributes = new CharacterAttributes()
            {
                strength = attributes.strength,
                dexterity = attributes.dexterity,
                intelligence = attributes.intelligence
            }
        };

    public static Status ConstructForEnemies (int level, int strength, int dexterity, int intelligence)
        => new Status
        {
            level = level,
            attributes = new CharacterAttributes
            {
                strength = strength,
                dexterity = dexterity,
                intelligence = intelligence
            }
        };

    protected bool Equals (Status other)
        => Strength == other.Strength
           && Dexterity == other.Dexterity
           && Intelligence == other.Intelligence;

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Status)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Strength;
            hashCode = (hashCode * 397) ^ Dexterity;
            hashCode = (hashCode * 397) ^ Intelligence;
            return hashCode;
        }
    }

    public static Status operator + (Status left, Status right)
        => new Status
        {
            experience = left.experience + right.experience,
            attributes = left.attributes + right.attributes
        };

    public static bool operator == (Status left, RequirementsStatus right)
        => left.Strength == right.attributes.strength;

    public static bool operator != (Status left, RequirementsStatus right)
        => !(left == right);

    public static bool operator > (Status left, RequirementsStatus right)
        => left.Strength > right.attributes.strength
           && left.Dexterity > right.attributes.dexterity
           && left.Intelligence > right.attributes.intelligence
           && left.level > right.level;

    public static bool operator < (Status left, RequirementsStatus right)
        => left.Strength < right.attributes.strength
           && left.Dexterity < right.attributes.dexterity
           && left.Intelligence < right.attributes.intelligence
           && left.level < right.level;

    public static bool operator >= (Status left, RequirementsStatus right)
        => left > right || left == right;

    public static bool operator <= (Status left, RequirementsStatus right)
        => left < right || left == right;

    public int CompareTo (Status other)
    {
        if (ReferenceEquals(this, other))
            return 0;
        if (ReferenceEquals(null, other))
            return 1;
        var strengthComparison = Strength.CompareTo(other.Strength);
        if (strengthComparison != 0)
            return strengthComparison;
        var dexterityComparison = Dexterity.CompareTo(other.Dexterity);
        if (dexterityComparison != 0)
            return dexterityComparison;
        return Intelligence.CompareTo(other.Intelligence);
    }
}