using System.Collections;

public class InventoryEquipmentSlot : SlotEntry
{
    public override IEnumerator Setup (SlotEntrySetupContext context)
    {
        yield return base.Setup(context);
    }
}