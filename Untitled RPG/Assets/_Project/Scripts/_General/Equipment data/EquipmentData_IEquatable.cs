public abstract partial class EquipmentData
{
    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((EquipmentData)obj);
    }

    public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;
    public static bool operator == (EquipmentData left, EquipmentData right) => Equals(left, right);
    public static bool operator != (EquipmentData left, EquipmentData right) => !Equals(left, right);
}