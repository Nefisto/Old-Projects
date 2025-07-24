using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : SlotEntry
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text label;

    [TitleGroup("Settings")]
    [SerializeField]
    private EquipmentKind equipmentKind;

    public event Action OnUpdateSlot;

    public override IEnumerator Setup (SlotEntrySetupContext context)
    {
        yield return base.Setup(context);

        AddClickBehavior(ClickBehavior);
        OnUpdateSlot = context.OnUpdateSlot;
    }

    protected override void ClearBehaviors()
    {
        base.ClearBehaviors();
        OnUpdateSlot?.Invoke();
    }

    public override void RefreshView (EquipmentData equipmentData)
    {
        base.RefreshView(equipmentData);

        label.text = equipmentData.Name;
    }

    private void ClickBehavior (BaseEventData data)
    {
        var playerData = ServiceLocator.SessionManager.PlayableCharacterData;
        var currentEquipped = equipmentKind switch
        {
            EquipmentKind.Weapon => playerData.CurrentEquipment.CurrentWeapon as
                EquipmentData,
            EquipmentKind.Armor => playerData.CurrentEquipment.CurrentArmor,
            _ => throw new ArgumentOutOfRangeException()
        };

        GameEvents.onOpenInventory?.Invoke(new InventorySetupContext(equipmentKind, currentEquipped)
        {
            entryInteractions = new SlotClickInteractions()
            {
                doubleClick = selectedEquipment =>
                {
                    playerData.CurrentEquipment.Equip(selectedEquipment);
                    RefreshView(selectedEquipment);
                    OnUpdateSlot?.Invoke();
                }
            }
        });
    }
}