#if UNITY_EDITOR
using Sirenix.OdinInspector;

public partial class BattleResultHUD
{
    [TitleGroup("Debug")]
    [Button]
    [DisableInEditorMode]
    public void DebugRun (BattleResultData resultData) => StartCoroutine(Run(resultData));
}
#endif