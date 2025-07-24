using System;

public partial class Template
{
    public bool Equals (Template other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(traitChart, other.traitChart)
               && Equals(currentEquipment, other.currentEquipment)
               && characterIconEnum == other.characterIconEnum;
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Template)obj);
    }

    public override int GetHashCode() => HashCode.Combine(traitChart, currentEquipment, (int)characterIconEnum);

    public static bool operator == (Template left, Template right) => Equals(left, right);

    public static bool operator != (Template left, Template right) => !Equals(left, right);
}