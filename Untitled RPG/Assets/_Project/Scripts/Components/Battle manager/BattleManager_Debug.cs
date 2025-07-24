#if UNITY_EDITOR
using NTools;
using Sirenix.OdinInspector;

public partial class BattleManager
{
    [Button]
    [DisableInEditorMode]
    private void TriggerBattle (BattleSetupContext setupContext)
        => battleRoutine = new NTask(SetupBattle(setupContext));

    [Button]
    [DisableInEditorMode]
    private void StopBattle() => battleRoutine.Stop();
}
#endif