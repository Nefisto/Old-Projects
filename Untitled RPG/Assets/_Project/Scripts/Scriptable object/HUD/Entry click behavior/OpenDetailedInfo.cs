using UnityEngine;

[CreateAssetMenu(fileName = "Click behavior", menuName = EditorConstants.MenuAssets.CLICK_BEHAVIOR + "Open detailed info", order = 0)]
public class OpenDetailedInfo : ClickBehavior
{
    protected override void Behavior (ClickBehaviorContext ctx) => GameEvents.RaiseEquipmentInfo(ctx.equipmentData);
}