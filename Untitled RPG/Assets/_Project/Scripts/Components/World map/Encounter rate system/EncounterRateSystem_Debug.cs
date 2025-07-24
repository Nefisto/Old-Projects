using Sirenix.OdinInspector;
using UnityEngine;

public partial class EncounterRateSystem
{
    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public void SetEncounterRate ([MinValue(0f)] [MaxValue(1f)] float percentage = 1f)
        => tickResource.Current = Mathf.RoundToInt(tickResource.CurrentMax * percentage);

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public void CheckBattle() => BattleCheck();
}