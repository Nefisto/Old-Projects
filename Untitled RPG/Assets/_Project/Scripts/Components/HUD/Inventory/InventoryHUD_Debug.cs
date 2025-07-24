#if UNITY_EDITOR
using Sirenix.OdinInspector;

public partial class InventoryHUD
{
    [TitleGroup("Debug")]
    [DisableInEditorButton]
    private void Test_Setup() => StartCoroutine(Setup());
}
#endif