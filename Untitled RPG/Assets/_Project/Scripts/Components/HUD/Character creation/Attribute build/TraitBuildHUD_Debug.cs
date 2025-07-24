#if DEBUG_CODE
public partial class TraitBuildHUD
{
    [DisableInEditorButton]
    public void Close() => (this as IMenu).Close();

    [DisableInEditorButton]
    public void Open() => (this as IMenu).Open();
}
#endif