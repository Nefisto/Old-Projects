using System;
using System.Collections;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WanderState : EnemyState
{
    // Task that control wander behavior 
    private Task wanderTask;
    
    private Vector3 targetPosition;
    
    #region Override
    
    public override EnemyStates EnumEnemyState => EnemyStates.Wander;
    public WanderState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        Think();
    }

    public override void Exit()
    {
        base.Exit();
        
        enemy.StopAllCoroutines();

        enemy.DOKill();
        enemy.transform.DOKill();
        
        wanderTask.Stop();
        
        // Set speed to 0
        enemy.animator.SetFloat(SpeedX, 0f);
        enemy.animator.SetFloat(SpeedY, 0f);
    }

    protected virtual void Wander()
    {
        // Cache
        var enemyPosition = enemy.transform.position;
        
        // Update action (Debug reasons)
        enemy.ChangeAction("Wandering");

        // Randomize a position to move
        // var targetPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        targetPosition = GetValidPosition(out var path);
            
        // Update animation
        var speedDirection = targetPosition - enemyPosition.NTIgnoreY();
        enemy.UpdateMovementAnimation(speedDirection);
        
        // enemy.navMeshAgent.SetDestination(targetPosition);
        wanderTask = enemy.navMeshAgent.SetPathWithEndCallback(path, () => enemy.ChangeState(EnemyStates.Idle));
    }

#if UNITY_EDITOR
    public override void GizmosUpdate()
    {
        base.GizmosUpdate();

        Handles.color = Color.green;
        Handles.DrawAAPolyLine(Texture2D.whiteTexture, enemy.navMeshAgent.path.corners);
    }
#endif
    

    #endregion
    
    // This will decide what will be the next step
    protected virtual void Think()
    {
        // For now we just wander
        Wander();
    }
    
    private Vector3 GetValidPosition(out NavMeshPath validPath)
    {
        var counter = 0;
        validPath = new NavMeshPath();
        
        while (true)
        {
            if (++counter > 20)
            {
                Debug.LogError("Character can't find a valid path", enemy);
                return Vector3.zero;
            }

            targetPosition = enemy.wanderArea.NRandomizePosition();
            enemy.navMeshAgent.CalculatePath(targetPosition.NTIgnoreY(), validPath);
            
            if (validPath.status == NavMeshPathStatus.PathComplete)
            {
                return targetPosition;
            }
        }
    }

    private static readonly int SpeedX = Animator.StringToHash("Speed_X");
    private static readonly int SpeedY = Animator.StringToHash("Speed_Y");
}