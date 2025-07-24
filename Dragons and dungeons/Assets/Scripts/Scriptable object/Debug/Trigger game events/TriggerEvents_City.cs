using Sirenix.OdinInspector;

public partial class TriggerEvents
{
    [Title("City")]
    [DisableInEditorMode]
    [Button]
    public void StartCity()
        => GameEvents.City.RaiseStart();
}