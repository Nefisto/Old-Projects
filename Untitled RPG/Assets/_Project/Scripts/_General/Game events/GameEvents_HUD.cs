using System;
using NTools;
using Sirenix.OdinInspector;

public partial class GameEvents
{
    public static Action<MenuSetupContext> onOpenInventory { get; set; }
    public static EntryPoint<SkillDetailedInfo.Settings> OpenDetailedSkillInfoEntryPoint { get; set; } = new();
    public static EntryPoint<GameJob> OpenGameJobViewEntryPoint { get; set; } = new();

    public static event Action<EquipmentData> OnOpenEquipmentInfo;

    [TitleGroup("HUD")]
    [DisableInEditorButton]
    public static void RaiseEquipmentInfo (EquipmentData data) => OnOpenEquipmentInfo?.Invoke(data);

    [TitleGroup("HUD")]
    [DisableInEditorButton]
    private void RaiseOpenInventory (MenuSetupContext context = default) => onOpenInventory?.Invoke(context);
}