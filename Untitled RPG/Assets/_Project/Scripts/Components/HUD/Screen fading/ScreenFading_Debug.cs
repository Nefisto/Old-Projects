using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class ScreenFading
{
    [Button]
    [DisableInEditorMode]
    private void Test_FadeOut(IScreenFading.Settings settings) => StartCoroutine(FadeOut(settings));

    [DisableInEditorButton]
    private void Test_FadeIn (IScreenFading.Settings settings) => StartCoroutine(FadeIn(settings));
}
#endif