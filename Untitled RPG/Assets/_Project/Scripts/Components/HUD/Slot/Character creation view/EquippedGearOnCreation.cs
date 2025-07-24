using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class EquippedGearOnCreation : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private EquipmentSlotOnCreationView weaponSlot;

    [TitleGroup("References")]
    [SerializeField]
    private EquipmentSlotOnCreationView armorSlot;

    public IEnumerator Setup (CurrentEquipment currentEquipment)
    {
        yield return weaponSlot.Setup(new SlotEntrySetupContext()
        {
            clickInteractions = new SlotClickInteractions()
            {
                doubleClick = currentEquipment.Equip
            },
            equipmentData = currentEquipment.CurrentWeapon
        });

        yield return armorSlot.Setup(new SlotEntrySetupContext()
        {
            clickInteractions = new SlotClickInteractions()
            {
                doubleClick = currentEquipment.Equip
            },
            equipmentData = currentEquipment.CurrentArmor
        });
    }

    public void RefreshGear (CurrentEquipment equipment)
    {
        weaponSlot.RefreshView(equipment.CurrentWeapon);
        armorSlot.RefreshView(equipment.CurrentArmor);
    }
}