using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public partial class Enemy
{
    #region SM Stuff

    [TabGroup("Behavior SM")]
    [Title("Settings")]
    [SerializeField]
    private bool startPaused;

    #region Idle

    [TabGroup("Behavior SM")]
    [Title("States settings")]
    [Title("Idle")]
    [Tooltip("After tickValue enemy will think in a new state to transition to")]
    [MinMaxSlider(.5f, 2f)]
    public Vector2 tickValue = new Vector2(1f, 1f);

    #endregion

    #region Wander

    [TabGroup("Behavior SM")]
    [Required]
    [Title("Wander")]
    public BoxCollider wanderArea;
    
    [TabGroup("Behavior SM")]
    public float wanderSpeed = 5f;
    
    [TabGroup("Behavior SM")]
    public Ease wanderEase = Ease.InOutSine;

    [TabGroup("Behavior SM")]
    [Tooltip("This will change the speed that enemy rotate when wandering")]
    public float rotateSpeed = 3f;

    #endregion

    #region Flee

    [TabGroup("Behavior SM")]
    [Title("Flee")]
    [Tooltip("How fast the enemy will flee from player")]
    public float fleeSpeed = 5f;

    [TabGroup("Behavior SM")]
    public Ease fleeEase = Ease.OutSine;

    #endregion
    
    private EnemyStateMachine behaviorMachine;
    private IdleState idleState;
    private WanderState wanderState;
    private FleeState fleeState;
    private DieState dieState;

    #endregion

    #region Debug fields

    [FormerlySerializedAs("targetState")]
    [TabGroup("Behavior SM")]
    [Title("Debug")]
    [DisableInEditorMode]
    [InlineButton("ChangeState", "Change to")]
    [SerializeField]
    private EnemyStates targetEnemyState;

#if UNITY_EDITOR
    [FormerlySerializedAs("currentState")]
    [TabGroup("Behavior SM")]
    [ReadOnly]
    public EnemyStates currentEnemyState;
#endif
    
    [TabGroup("Behavior SM")]
    [ReadOnly]
    [SerializeField]
    private string currentAction = "---";

    private PauseState pauseState;
    
    private bool isPaused;
    private EnemyState cachedState;

    #endregion
    
    #region API

    public void ChangeState (EnemyStates enumEnemyState)
    {
        EnemyState targetState = enumEnemyState switch
        {
            EnemyStates.Idle => idleState,
            EnemyStates.Wander => wanderState,
            EnemyStates.Flee => fleeState,
            EnemyStates.Pause => pauseState,
            EnemyStates.Die => dieState,
            _ => throw new ArgumentOutOfRangeException(nameof(enumEnemyState), enumEnemyState, null)
        };

        currentEnemyState = targetState.EnumEnemyState;
        behaviorMachine.ChangeState(targetState);
    }

    #endregion

    #region Private Methods

    private void SetupBehaviorMachine()
    {
        behaviorMachine = new EnemyStateMachine();

        idleState = new IdleState(this, behaviorMachine);
        wanderState = new WanderState(this, behaviorMachine);
        fleeState = new FleeState(this, behaviorMachine);
        pauseState = new PauseState(this, behaviorMachine);
        dieState = new DieState(this, behaviorMachine);
        
        var initialState = startPaused ? (EnemyState)pauseState : idleState;
        currentEnemyState = initialState.EnumEnemyState;
        behaviorMachine.Initialize(initialState);
    }

    #endregion

    #region Debug methods

    [TabGroup("Behavior SM")]
    [DisableInEditorMode]
    [Button("Pause/Unpause SM")]
    private void PauseSMToggle()
    {
        if (!isPaused)
        {
            cachedState = behaviorMachine.CurrentState;

            ChangeState(EnemyStates.Pause);
        }
        else
        {
            ChangeState(cachedState.ToEnumStates());
        }

        isPaused = !isPaused;
    }

    public void ChangeAction (string action = "---")
    {
        currentAction = action;
    }

    #endregion
}