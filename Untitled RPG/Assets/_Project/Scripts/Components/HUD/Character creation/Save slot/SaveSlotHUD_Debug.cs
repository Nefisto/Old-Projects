using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class SaveSlotHUD
{
    [DisableInEditorButton]
    private void T_LoadData() => StartCoroutine(LoadSlot());

    [DisableInEditorButton]
    private void T_SaveData (TemplateDataFactory template) => StartCoroutine(SaveSlot(template.GetInstance()));

    [DisableInEditorButton]
    private void T_EraseSlot() => StartCoroutine(EraseSlot());

    [Button]
    private void T_EraseSave() => StartCoroutine(EraseSave());

    [DisableInEditorButton]
    private void T_EmptySlot() => StartCoroutine(SetupEmptySlot());
}
#endif