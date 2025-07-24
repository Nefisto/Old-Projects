public class NullSelector : ITargetSelector
{
    public EnemyBattleActor CurrentTarget => null;

    public void SetTarget (EnemyBattleActor target) { }

    public void Clear() { }
}