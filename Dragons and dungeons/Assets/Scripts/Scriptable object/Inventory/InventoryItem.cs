using System;

/// <summary>
/// Layer to control current amount of item
/// </summary>
[Serializable]
public class InventoryItem : IEquatable<InventoryItem>, ICloneable
{
    public ItemData data;

    public int amount = 1;

    public bool CanStack => data != null && data.CanStack;
    
    public InventoryItem() { }

    public InventoryItem(ItemData data)
        => this.data = data;

    public InventoryItem (ItemData data, int amount)
    {
        this.data = data;
        this.amount = amount;
    }

    public InventoryItem (InventoryItem other)
    {
        if (other == null)
            return;

        data = other.data;
        amount = other.amount;
    }

    public bool Equals (InventoryItem other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(data, other.data);
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((InventoryItem)obj);
    }

    public override int GetHashCode()
        => (data != null ? data.GetHashCode() : 0);

    public object Clone()
    {
        var clone = (InventoryItem)MemberwiseClone();

        clone.data = data != null ? (ItemData)data.Clone() : null;

        return clone;
    }

    public static bool operator == (InventoryItem left, InventoryItem right)
        => Equals(left, right);
    
    public static bool operator != (InventoryItem left, InventoryItem right)
        => !Equals(left, right);
}