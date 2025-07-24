using System;

[Serializable]
public class Equipment
{
    public Weapon weapon;
    public ChestArmor chestArmor;
    public HeadArmor headArmor;

    public Equipment() { }

    public Equipment(Equipment other)
    {
        if (other == null)
            return;
        
        if (other.weapon != null)
            weapon = (Weapon)other.weapon.Clone();
        
        if (other.chestArmor != null)
            chestArmor = (ChestArmor)other.chestArmor.Clone();
        
        if (other.headArmor != null)
            headArmor = (HeadArmor)other.headArmor.Clone();
    }
}