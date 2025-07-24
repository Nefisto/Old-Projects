using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ItemData : ScriptableObject, ICloneable
{
    [Title("Information")]
    [InfoBox("The name is optional, if you left it blank the SO name will be loaded on runtime")]
    public string itemName;

    [Multiline]
    public string description;
    
    [TitleGroup("Graphics")]
    [PreviewField]
    [SerializeField]
    protected Sprite icon;

    [ShowIf(@"CanStack")]
    [Min(0)]
    public int maxStack = 1;

    public virtual Sprite GetIconSprite => icon;

    public abstract bool CanStack { get; protected set; }

    protected virtual ItemData CreateNewInstance => CreateInstance<ItemData>();

    public virtual object Clone()
    {
        var clone = CreateNewInstance;

        clone.icon = icon;
        
        var nameToCopy = !string.IsNullOrWhiteSpace(itemName) ? itemName : name;
        clone.itemName = string.Copy(nameToCopy);
        clone.description = string.Copy(description);

        return clone;
    }

    protected bool Equals (ItemData other)
        => base.Equals(other) && itemName == other.itemName;

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ItemData)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (itemName != null ? itemName.GetHashCode() : 0);
        }
    }

    public static bool operator == (ItemData left, ItemData right)
    {
        if (ReferenceEquals(left, right))
            return true;
        
        if (ReferenceEquals(null, left) || ReferenceEquals(null, right))
            return false;
        
        return left.itemName == right.itemName;
    }

    public static bool operator != (ItemData left, ItemData right)
        => !(left == right);
}