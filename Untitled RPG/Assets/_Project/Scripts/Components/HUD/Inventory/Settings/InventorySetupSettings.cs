using System;

[Serializable]
public class InventorySetupContext : MenuSetupContext
{
    public EquipmentKind equipmentKind;

    public Action close;
    public EquipmentData currentEquipped;
    public SlotClickInteractions entryInteractions;

    public InventorySetupContext (EquipmentKind equipmentKind, EquipmentData currentEquipped)
    {
        this.equipmentKind = equipmentKind;
        this.currentEquipped = currentEquipped;
    }
}