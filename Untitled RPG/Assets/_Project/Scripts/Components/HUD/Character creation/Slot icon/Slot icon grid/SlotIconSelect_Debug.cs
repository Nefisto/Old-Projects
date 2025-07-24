#if UNITY_EDITOR

public partial class SlotIconGrid
{
    [DisableInEditorButton]
    private void T_Setup() => StartCoroutine(Setup());
}
#endif