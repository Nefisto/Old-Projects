using System;

public partial class CurrentEquipment
{
    public bool Equals (CurrentEquipment other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(CurrentWeapon, other.CurrentWeapon)
               && Equals(CurrentArmor, other.CurrentArmor);
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((CurrentEquipment)obj);
    }

    public override int GetHashCode() => HashCode.Combine(CurrentWeapon, CurrentArmor);

    public static bool operator == (CurrentEquipment left, CurrentEquipment right) => Equals(left, right);

    public static bool operator != (CurrentEquipment left, CurrentEquipment right) => !Equals(left, right);
}