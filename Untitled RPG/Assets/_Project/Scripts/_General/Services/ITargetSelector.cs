public interface ITargetSelector
{
    public EnemyBattleActor CurrentTarget { get; }

    public bool HasSelectedTarget => CurrentTarget != null;

    public void SetTarget (EnemyBattleActor target);
    public void Clear();
}