using System;
using Cinemachine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

public partial class Player
{
    private const string Tab_Camera = "Aim";

    [TabGroup(Tab_Camera)]
    [PropertyTooltip("Yellow: Screen -> World\nPink: Flashlight offset -> target")]
    [NonSerialized, OdinSerialize]
    public bool debugAim;

    [TabGroup(Tab_Camera)]
    [Title("Control")]
    [PropertyTooltip("Standard virtual camera")]
    [OdinSerialize]
    public GameObject standardCamera { get; private set; }

    [TabGroup(Tab_Camera)]
    [PropertyTooltip("Aim virtual camera")]
    [OdinSerialize]
    public GameObject AimCamera { get; private set; }

    [TabGroup(Tab_Camera)]
    [Title("Debug")]
    [ReadOnly]
    [OdinSerialize]
    public Canvas AimCanvas { get; private set; }

    [TabGroup(Tab_Camera)]
    [ReadOnly]
    [OdinSerialize]
    public Image AimSymbol { get; private set; }

    public void Aim()
    {
        standardCamera.SetActive(false);
        AimCamera.SetActive(true);
    }

    public void Standard()
    {
        standardCamera.SetActive(true);
        AimCamera.SetActive(false);
    }

    private void SetupAim()
    {
        AimCanvas = GameObject.FindWithTag("HUD")?.GetComponent<Canvas>()
                    ?? throw new Exception("Can't found canvas");

        AimSymbol = GameObject.FindWithTag("Aim image")?.GetComponent<Image>()
                    ?? throw new Exception("Can't find the aim symbol");
    }

    private void RegisterAimInputs()
    {
        var aimInput = InputController.Input.Gameplay.Aim;

        aimInput.performed += _ => stateMachine.ChangeState(aiming);
    }

    private void DebugAim()
    {
        if (!Application.isPlaying)
            return;
        
        if (stateMachine.CurrentState.playerState == PlayerStates.Aiming)
        {
            stateMachine.CurrentState.GizmosUpdate();
        }
    }
}