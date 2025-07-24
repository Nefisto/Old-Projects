using System;

[Serializable]
public class ArmorData : EquipmentData
{
    public override EquipmentKind EquipmentKind => EquipmentKind.Armor;
}