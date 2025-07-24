using System;
using UnityEngine;

[Serializable]
public class ManaPool
{
    public int red;
    public int green;
    public int blue;

    public void Add (ManaName mana, int amount)
    {
        switch (mana)
        {
            case ManaName.Red:
                AddRed(amount);
                break;

            case ManaName.Green:
                AddGreen(amount);
                break;

            case ManaName.Blue:
                AddBlue(amount);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(mana), mana, null);
        }
    }

    private void AddRed (int amount)
        => red += amount;

    private void AddGreen (int amount)
        => green += amount;

    private void AddBlue (int amount)
        => blue += amount;
    
    protected bool Equals (ManaPool other)
        => red == other.red && green == other.green && blue == other.blue;

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        
        return obj.GetType() == GetType() 
               && Equals((ManaPool)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = red;
            hashCode = (hashCode * 397) ^ green;
            hashCode = (hashCode * 397) ^ blue;
            return hashCode;
        }
    }

    public static bool operator == (ManaPool left, ManaPool right)
    {
        return Equals(left, right);
    }

    public static bool operator != (ManaPool left, ManaPool right)
    {
        return !Equals(left, right);
    }

    public static ManaPool operator + (ManaPool left, ManaPool right)
        => new ManaPool
        {
            red = Mathf.Clamp(left.red + right.red, 0, GameConstants.Character.MaxAmountOfMana),
            green = Mathf.Clamp(left.green + right.green, 0, GameConstants.Character.MaxAmountOfMana),
            blue = Mathf.Clamp(left.blue + right.blue, 0, GameConstants.Character.MaxAmountOfMana)
        };

    public static ManaPool operator - (ManaPool left, ManaPool right)
        => new ManaPool
        {
            red = Mathf.Clamp(left.red - right.red, 0, GameConstants.Character.MaxAmountOfMana),
            green = Mathf.Clamp(left.green - right.green, 0, GameConstants.Character.MaxAmountOfMana),
            blue = Mathf.Clamp(left.blue - right.blue, 0, GameConstants.Character.MaxAmountOfMana),
        };

    public static bool operator <= (ManaPool left, ManaPool right)
        => left.red <= right.red 
           || left.green <= right.green
           || left.blue <= right.blue;

    public static bool operator >= (ManaPool left, ManaPool right)
        => left.red >= right.red
           && left.green >= right.green
           && left.blue >= right.blue;

    public static bool operator < (ManaPool left, ManaPool right)
        => left.red < right.red 
           || left.green < right.green
           || left.blue < right.blue;

    public static bool operator > (ManaPool left, ManaPool right)
        => left.red > right.red
           && left.green > right.green
           && left.blue > right.blue;

    public override string ToString()
        => $"Red: {red} - Green: {green} - Blue: {blue}";
}