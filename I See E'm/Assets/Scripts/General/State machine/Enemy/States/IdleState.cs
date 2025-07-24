using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class IdleState : EnemyState
{
    private TweenerCore<float, float, FloatOptions> thinkReference;

    public override EnemyStates EnumEnemyState => EnemyStates.Idle;
    
    public IdleState (Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        Think();
    }

    public override void Exit()
    {
        base.Exit();

        thinkReference.Kill();
    }

    private void Think()
    {
        enemy.ChangeAction("Thinking");
        
        var tickTime = enemy.tickValue.GetRandom();

        var a = 0f;
        thinkReference = DOTween.To(() => a, x => a = x, tickTime, tickTime)
            .OnComplete(ChangeState);
    }

    private void ChangeState()
    {
        var rand = Random.value;

        if (rand <= 1f)
        {
            enemy.ChangeState(EnemyStates.Wander);
        }
        
    }
}