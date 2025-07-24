using Sirenix.OdinInspector;

public partial class InventoryHUD
{
    [TitleGroup("Debug")]
    [DisableInEditorButton]
    public void Open (MenuSetupContext context = null)
    {
        gameObject.SetActive(true);

        StartCoroutine(Setup(context));
    }

    [TitleGroup("Debug")]
    [DisableInEditorButton]
    public void Close()
    {
        gameObject.SetActive(false);

        OnClose?.Invoke();

        Clean();
    }
}