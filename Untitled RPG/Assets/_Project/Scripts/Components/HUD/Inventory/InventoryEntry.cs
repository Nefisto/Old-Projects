using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotClickInteractions
{
    public Action<EquipmentData> doubleClick;
}

public class InventoryEntrySetupContext : SlotEntrySetupContext
{
    public bool isEquippedSlot;
}

public class InventoryEntry : SlotEntry
{
    [TitleGroup("References")]
    [SerializeField]
    protected Image background;

    private EquipmentData cachedEquipment;

    private Action<EquipmentData> doubleClickAnswer;

    public override IEnumerator Setup (SlotEntrySetupContext context)
    {
        var correctContext = (InventoryEntrySetupContext)context;
        cachedEquipment = correctContext.equipmentData;

        yield return base.Setup(context);

        AddClickBehavior(data => DoubleClickBehavior(data, correctContext.clickInteractions?.doubleClick));
        AddHoldingBehavior(HoldingBehavior);

        SetEquippedSlot(correctContext.isEquippedSlot);
    }

    private void DoubleClickBehavior (BaseEventData data, Action<EquipmentData> doubleClickAnswer)
    {
        var pointEventData = data as PointerEventData;
        if (pointEventData.clickCount < 2)
            return;

        doubleClickAnswer?.Invoke(cachedEquipment);

        ServiceLocator.MenuStack.CloseMenu();
    }

    private IEnumerator HoldingBehavior (BaseEventData data)
    {
        yield return new WaitForSeconds(GameConstants.HOLDING_SECONDS_TO_SHOW_INFO);

        GameEvents.RaiseEquipmentInfo(cachedEquipment);
    }

    private void SetEquippedSlot (bool isEquipped) => background.color = isEquipped ? Color.red : Color.white;
}