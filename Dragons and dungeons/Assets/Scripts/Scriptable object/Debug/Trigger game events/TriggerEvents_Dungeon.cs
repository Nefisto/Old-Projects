using Sirenix.OdinInspector;

public partial class TriggerEvents
{
    [Title("DungeonMap")]
    [DisableInEditorMode]
    [Button]
    public void FirstSetupDungeon()
        => GameEvents.DungeonMap.RaiseFirstSetupDungeon();

    [DisableInEditorMode]
    [Button]
    public void StartDungeon()
        => GameEvents.DungeonMap.RaiseStartDungeon();

    [DisableInEditorMode]
    [Button]
    public void PreResumeDungeon()
        => GameEvents.DungeonMap.RaisePreToResumeDungeon();

    [DisableInEditorMode]
    [Button]
    public void ResumeDungeon()
        => GameEvents.DungeonMap.RaiseResumeDungeon();

    [DisableInEditorMode]
    [Button]
    public void FinishDungeon()
        => GameEvents.DungeonMap.RaiseFinishDungeon();
}