using Sirenix.OdinInspector;
using UnityEngine;

public partial class DebugSO
{
    [TabGroup("Combat log")]
    [Title("Settings")]
    [SerializeField]
    private string message;

    [TabGroup("Combat log")]
    [DisableInEditorMode]
    [Button]
    private void RaiseCombatLogEvent()
    {
        GameEvents.Battle.RaiseCombatLogAction(new CustomCombatLog(message));
    }
}