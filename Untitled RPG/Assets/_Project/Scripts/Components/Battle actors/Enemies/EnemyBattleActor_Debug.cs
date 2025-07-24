using Sirenix.OdinInspector;

#if UNITY_EDITOR
// ReSharper disable UnusedMember.Local

public partial class EnemyBattleActor
{
    [TitleGroup("Debug")]
    [DisableInEditorButton]
    private void Setup (EnemyDataFactory enemyTemplate)
    {
        SetupBattleStart(new SetupBattleActorContext { data = enemyTemplate.GetInstance() });
    }
}
#endif