using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public partial class InventoryHUD : MonoBehaviour, IMenu
{
    [TitleGroup("Settings")]
    [SerializeField]
    private InventoryEntry entryPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private ScrollRect scrollRect;

    [Tooltip("Just to see the last setup on inspector, debug purposes only")]
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private InventorySetupContext lastSetupContext;

    private void Awake() => GameEvents.onOpenInventory += settings => ServiceLocator.MenuStack.OpenMenu(this, settings);

    private event Action OnClose;

    private IEnumerator Setup (MenuSetupContext context = null)
    {
        lastSetupContext = (InventorySetupContext)(context ?? GetDefaultSettings());

        OnClose = lastSetupContext.close;

        var (correctEntries, currentEquipped) = lastSetupContext.equipmentKind switch
        {
            EquipmentKind.Weapon => (Database.Weapons.InstanceData.Cast<EquipmentData>(),
                lastSetupContext.currentEquipped),
            EquipmentKind.Armor => (Database.Armors.InstanceData, lastSetupContext.currentEquipped),
            _ => throw new ArgumentOutOfRangeException()
        };

        ClearEntries();
        yield return FillEntries(correctEntries);

        IEnumerator FillEntries (IEnumerable<EquipmentData> instances)
        {
            var content = scrollRect.content;
            foreach (var equipmentData in instances)
            {
                var entry = Instantiate(entryPrefab, content, false);
                yield return entry.Setup(new InventoryEntrySetupContext
                {
                    equipmentData = equipmentData,
                    isEquippedSlot = equipmentData == currentEquipped,
                    clickInteractions = lastSetupContext.entryInteractions
                });
            }
        }
    }

    private static InventorySetupContext GetDefaultSettings() => new(EquipmentKind.Weapon, null);

    private void ClearEntries()
    {
        foreach (var child in scrollRect.content.Cast<Transform>())
            Destroy(child.gameObject);
    }

    private void Clean() => OnClose = null;
}