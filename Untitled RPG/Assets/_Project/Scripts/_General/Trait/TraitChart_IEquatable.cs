using System;

public partial class TraitChart
{
    public bool Equals (TraitChart other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return level == other.level
               && Equals(StrengthSector, other.StrengthSector)
               && Equals(VitalitySector, other.VitalitySector)
               && Equals(DexteritySector, other.DexteritySector)
               && Equals(IntelligenceSector, other.IntelligenceSector);
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((TraitChart)obj);
    }

    public override int GetHashCode()
        => HashCode.Combine(level, StrengthSector, VitalitySector, DexteritySector, IntelligenceSector);

    public static bool operator == (TraitChart left, TraitChart right) => Equals(left, right);
    public static bool operator != (TraitChart left, TraitChart right) => !Equals(left, right);
}