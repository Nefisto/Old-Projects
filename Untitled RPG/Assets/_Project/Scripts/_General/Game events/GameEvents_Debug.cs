using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class GameEvents
{
    [BoxGroup("Gameplay")]
    [DisableInEditorButton]
    private void T_GameOver() => OnGameOver?.Invoke();

    [BoxGroup("Game over screen")]
    [InfoBox("After death, selecting to retry adventure using the same template")]
    [DisableInEditorButton]
    private void T_Retry() => OnGameOverRetryButtonPressed?.Invoke();

    [Button]
    [DisableInEditorMode]
    private static void T_Pause() => OnPause?.Invoke();

    [Button]
    [DisableInEditorMode]
    private static void T_Unpause() => OnUnpause?.Invoke();

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    private static void T_TickEncounter() => OnTickEncounter?.Invoke();
}
#endif