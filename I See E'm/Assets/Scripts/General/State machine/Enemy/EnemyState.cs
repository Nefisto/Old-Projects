using System;
using UnityEditor;
using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    abstract public EnemyStates EnumEnemyState { get; }
        
    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void HandleInput()
    {
        
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void GizmosUpdate()
    {
        
    }
    
    public virtual void Exit()
    {

    }
}