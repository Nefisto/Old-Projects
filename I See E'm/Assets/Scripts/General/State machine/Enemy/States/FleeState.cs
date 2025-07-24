using System.Timers;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
#if true
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

public class FleeState : EnemyState
{
    // The one that I'm fleeing off
    private Transform detector;

    private Vector3 fleeDirection;

    public override EnemyStates EnumEnemyState => EnemyStates.Flee;
    
    public FleeState (Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        enemy.ChangeAction("Fleeing");

        detector = enemy.GetNearScareDetectorTransform();

        MoveAwayFromPlayer();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.transform.DOKill();
    }

    private void Check()
    {
        if (!enemy.IsScared)
        {
            enemy.ChangeState(EnemyStates.Wander);
        }
    }

    private void MoveAwayFromPlayer()
    {
        var enemyPosition = enemy.transform.position;
        
        // Calculate targetPosition
        fleeDirection = (enemyPosition.NTIgnoreY() - GetDetectorPosition()).normalized; // Go in inverse direction from player
        var targetPosition = enemyPosition + fleeDirection * 3f;
        
        // Update animation
        enemy.UpdateMovementAnimation(fleeDirection);
        enemy.UpdateLookAt(targetPosition);

        // Calculate flee duration
        var duration = Vector3.Distance(targetPosition, enemyPosition) / enemy.fleeSpeed;

        enemy.transform
            .DOMove(targetPosition, duration)
            .SetEase(enemy.fleeEase)
            .OnComplete(Check);
    }

    /// <summary>
    /// In debug we don't want to need for the player to exist in scenario
    /// </summary>
    /// <returns></returns>
    private Vector3 GetDetectorPosition()
        => detector?.position.NTIgnoreY() ?? Vector3.zero;
    
    
#if UNITY_EDITOR
    public override void GizmosUpdate()
    {
        base.GizmosUpdate();
        
        var position = enemy.transform.position;

        Handles.color = Color.red;
        Handles.DrawAAPolyLine(Texture2D.whiteTexture, position, position + fleeDirection);
    }
#endif
}