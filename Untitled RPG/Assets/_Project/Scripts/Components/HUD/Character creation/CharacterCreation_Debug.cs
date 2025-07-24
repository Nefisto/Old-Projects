using System.Collections;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class CharacterCreation2
{
    public void SkipCharacterCreation (Template template)
    {
        UpdateTemplate(template ?? Helper.LoadTemplate(TemporarySavePath));

        StartCoroutine(Behavior());

        IEnumerator Behavior()
        {
            Helper.SaveTemplate(TemporarySavePath, usedTemplate);
            DisableView();
            yield return GameEvents.OnBeginningAdventure?.YieldableInvoke(usedTemplate);
        }
    }

    [DisableInEditorButton]
    private void BeginSetup() => StartCoroutine(Setup(null));

    [DisableInEditorButton]
    private void LoadFromFactory (TemplateDataFactory dataFactory) => StartCoroutine(Setup(dataFactory.GetInstance()));

    [Button]
    private void EraseTemporarySave() => System.IO.File.Delete(TemporarySavePath);
}
#endif