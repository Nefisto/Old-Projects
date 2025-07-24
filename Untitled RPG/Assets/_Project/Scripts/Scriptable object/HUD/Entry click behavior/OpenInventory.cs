using UnityEngine;

[CreateAssetMenu(fileName = "Open inventory", menuName = EditorConstants.MenuAssets.CLICK_BEHAVIOR + "Open inventory",
    order = 0)]
public class OpenInventory : ClickBehavior
{
    protected override void Behavior (ClickBehaviorContext ctx)
    {
        GameEvents.onOpenInventory?.Invoke(new InventorySetupContext(EquipmentKind.Weapon, null));
    }
}