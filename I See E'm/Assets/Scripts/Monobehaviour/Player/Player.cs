using System;
using NTools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[SelectionBase]
public partial class Player : SerializedLazyBehavior
{
    #region Tabs n groups

    private const string FO_Skills = "Skills";
    private const string FO_TAB_Skills = "Skills/Skills";
    private const string TabName_Fireball = "Fireball";
    private const string TabName_Ray = "Ray";

    #endregion

    [TabGroup("Basic")]
    [Title("Control")]
    [PropertyTooltip("Offset for cast skills and detect things")]
    [Required]
    [OdinSerialize]
    public Transform FlashlightPosition { get; private set; }

    [TabGroup("Basic")]
    [Title("Masks")]
    [OdinSerialize]
    public LayerMask BlockableLayer { get; private set; }
    
    [TabGroup("Basic")]
    [OdinSerialize]
    public LayerMask EnemyLayer { get; private set; }

    private void Awake()
    {
        // Inputs
        RegisterMovementInputs();
        RegisterRotateInputs();
        RegisterAimInputs();
        RegisterFireballInput();
        RegisterRayInput();

        // Setup behaviours
        SetupEnemyDetection();
        SetupFuelUpdateTask();
        SetupRay();
        SetupAim(); // Get necessary references (Target image, Canvas to convert mouse pos)

        // Setup events
        RegisterFireballListeners();
        RegisterRayListeners();
        
        SetupStateMachine();
    }

    private void Start()
    {
        ResetFuel();
    }

    private void OnEnable()
    {
        OnUpdateFuel += UpdateAngle;
    }

    private void OnDisable()
    {
        OnUpdateFuel -= UpdateAngle;
    }

    private void Update()
    {
        stateMachine.CurrentState.HandleInput();

        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotate();
    }

    private void OnDrawGizmos()
    {
        if (debugRay)
            DebugRay();

        if (debugFieldOfView)
            DebugFoV();

        if (debugAim)
            DebugAim();
    }
}