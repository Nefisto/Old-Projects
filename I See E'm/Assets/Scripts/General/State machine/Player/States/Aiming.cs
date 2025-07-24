using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : PlayerState
{
    private static readonly int IsAiming = Animator.StringToHash("isAiming");

    private InputAction input => InputController.Input.Gameplay.Aim;

    #region Overrides

    public override PlayerStates playerState => PlayerStates.Aiming;

    public Aiming (Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // Disable system mouse
        Cursor.visible = false;

        // Set anim to aiming
        player.animator.SetBool(IsAiming, true);

        // Enable Aim image
        EnableAimSymbol();

        // TODO: Polish it
        player.Aim();

        // Register input
        input.canceled += CancelAiming;
    }

    public override void Exit()
    {
        base.Exit();

        // Re-enable cursor
        Cursor.visible = true;

        // Unset anim
        player.animator.SetBool(IsAiming, false);

        // Disable aim symbol
        DisableAimSymbol();

        // Reset aim symbol
        UpdateAimImagePosition(Vector3.zero);

        // Unregister event
        input.canceled -= CancelAiming;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateAimImagePosition();
    }

#if UNITY_EDITOR
    public override void GizmosUpdate()
    {
        base.GizmosUpdate();

        var originalColor = Handles.color;
        var gizmosOriginalColor = Gizmos.color;

        // From UI to world
        var screenToWorld = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(screenToWorld.origin, screenToWorld.direction * 10f, Color.yellow);

        // From flashlight
        var dir = GetDirectionFromFlashlight();
        var flashlightPosition = player.FlashlightPosition.position;
        Debug.DrawLine(flashlightPosition, flashlightPosition + dir * 10f, Color.magenta);

        if (Physics.Raycast(player.FlashlightPosition.position, dir, out var hit, 10f))
        {
            Gizmos.DrawSphere(hit.collider.transform.position, .5f);
        }

        Gizmos.color = gizmosOriginalColor;
        Handles.color = originalColor;
    }

    public override void CastFireball()
    {
        // Direction from flashlight to target
        var direction = GetDirectionFromFlashlight();
        player.CastFireballAtDirection(direction);
    }

    public override void CastRay()
    {
        player.CastRayAtDirection(GetDirectionFromFlashlight);
    }
#endif

    #endregion

    private void DisableAimSymbol()
        => player.AimSymbol.enabled = false;

    private void EnableAimSymbol()
        => player.AimSymbol.enabled = true;

    private void UpdateAimImagePosition()
    {
        player.AimSymbol.transform.position = GetMousePositionInScreen();
    }

    private void UpdateAimImagePosition (Vector3 position)
    {
        player.AimSymbol.transform.position = player.AimCanvas.transform.TransformPoint(position);
        ;
    }

    private Vector3 GetDirectionFromFlashlight()
    {
        // From UI to world
        var screenToWorld = Camera.main.ScreenPointToRay(Input.mousePosition);
        var endPoint = screenToWorld.origin + screenToWorld.direction * 10;

        return (endPoint - player.FlashlightPosition.position).normalized;
    }

    // This will convert your mouse position to a rect transform position to update the aim image
    private Vector3 GetMousePositionInScreen()
    {
        var a = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            player.AimCanvas.transform as RectTransform,
            Input.mousePosition,
            null,
            out var pos);

        return player.AimCanvas.transform.TransformPoint(pos);
    }

    private void CancelAiming (InputAction.CallbackContext ctx)
        => player.ChangeState(PlayerStates.Basic);
}