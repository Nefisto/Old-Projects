#if UNITY_EDITOR
using Sirenix.OdinInspector;

public partial class StatusMenuView
{
    [TitleGroup("Debug")]
    [DisableInEditorButton]
    private void Setup (PlayableCharacterDataFactory playableCharacterDataFactory)
        => StartCoroutine(Setup(playableCharacterDataFactory.GetInstance()));
}
#endif