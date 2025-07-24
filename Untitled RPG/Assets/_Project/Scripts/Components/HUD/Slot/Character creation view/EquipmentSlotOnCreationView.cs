using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class EquipmentSlotOnCreationView : SlotEntry
{
    [TitleGroup("Settings")]
    [SerializeField]
    private EquipmentKind equipmentKind;

    public override IEnumerator Setup (SlotEntrySetupContext context)
    {
        yield return base.Setup(context);

        RefreshView(context.equipmentData);
        var slotClickInteractions = context.clickInteractions ?? new SlotClickInteractions();
        slotClickInteractions.doubleClick += RefreshView;

        AddClickBehavior(data =>
        {
            GameEvents.onOpenInventory?.Invoke(new InventorySetupContext(equipmentKind, null)
            {
                entryInteractions = slotClickInteractions
            });
        });
    }
}