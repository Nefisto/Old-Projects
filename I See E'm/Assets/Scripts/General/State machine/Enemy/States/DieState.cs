using UnityEngine;

public class DieState : EnemyState
{
    private static readonly int Die = Animator.StringToHash("Die");
    public DieState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }
    public override EnemyStates EnumEnemyState => EnemyStates.Die;

    public override void Enter()
    {
        base.Enter();
        
        // Disable colliders
        enemy.GetComponent<BoxCollider>().enabled = false;

        // Stop navmesh
        enemy.navMeshAgent.isStopped = true;
        
        // Play die animation
        enemy.animator.SetTrigger(Die);

        // Start the Fade out process
        enemy.StartCoroutine(enemy.FadeOut());
        
        // Trigger die event
        enemy.OnEnemyDie.Invoke();
    }

    public override void Exit()
    {
        base.Exit();
    }
}