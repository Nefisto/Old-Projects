using System;
using Sirenix.OdinInspector;

[Serializable]
public class CharacterAttributes
{
    [PropertyOrder]
    public int health;
    
    [PropertyOrder(1)]
    public int strength;

    [PropertyOrder(1)]
    public int dexterity;

    [PropertyOrder(1)]
    public int intelligence;

    public static CharacterAttributes operator + (CharacterAttributes left, CharacterAttributes right)
        => new CharacterAttributes
        {
            health = left.health + right.health,
            strength = left.strength + right.strength,
            dexterity = left.dexterity + right.dexterity,
            intelligence = left.intelligence + right.intelligence
        };
}