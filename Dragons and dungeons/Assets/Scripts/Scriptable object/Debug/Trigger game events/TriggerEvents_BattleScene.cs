using Sirenix.OdinInspector;

public partial class TriggerEvents
{
    [Title("BattleScene")]
    [DisableInEditorMode]
    [Button]
    public void SetupBattle (Player player, EnemyGroup enemyGroup)
        => GameEvents.Battle.RaiseSetupBattle(new BattleEncounterContext
        {
            player = player,
            enemyGroup = enemyGroup
        });

    [DisableInEditorMode]
    [Button]
    public void StartBattle()
        => GameEvents.Battle.RaiseBattleStart();
}