using System;

public partial class Trait
{
    public bool Equals (Trait other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return grow == other.grow
               && points == other.points
               && attributeType == other.attributeType;
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Trait)obj);
    }

    public override int GetHashCode() => HashCode.Combine(grow, points, (int)attributeType);
    public static bool operator == (Trait left, Trait right) => Equals(left, right);
    public static bool operator != (Trait left, Trait right) => !Equals(left, right);
}