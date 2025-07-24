using UnityEngine;

// This exist only to allow me to pause the machine in a simple way
// This isn't exactly a PAUSE cause when unpause the SM will make the ENTER PHASE again on cached state
// BUT this is enough for now
public class PauseState : EnemyState
{
    public override EnemyStates EnumEnemyState => EnemyStates.Pause;
    
    public PauseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }
    
    public override void Enter()
    {
        enemy.ChangeAction("Paused");
    }

    public override void HandleInput()
    { }

    public override void LogicUpdate()
    { }

    public override void PhysicsUpdate()
    { }

    public override void GizmosUpdate()
    { }

    public override void Exit()
    { }
}