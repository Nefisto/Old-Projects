using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Player
{
    [TabGroup(TabNameMovement)]
    [Title("Status")]
    [SerializeField]
    private float movementSpeed = 5f;

    [TabGroup(TabNameMovement)]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool canMove = true;
    
    [TabGroup(TabNameMovement)]
    [ReadOnly]
    [SerializeField]
    private float moveDirection;

    #region API

    public void DisableMovement()
        => canMove = false;

    public void EnableMovement()
        => canMove = true;
    
    #endregion

    private void Movement()
    {
        if (!canMove)
            return;
        
        if (moveDirection.IsNearlyEnoughTo(0f))
            return;

        var velocity = transform.forward * moveDirection * movementSpeed;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        
        animator.SetFloat(Speed, moveDirection);
    }

    private void MovementCancel()
    {
        moveDirection = 0f;
        
        animator.SetFloat(Speed, 0f);
    }

    private void RegisterMovementInputs()
    {
        var input = InputController.Input;

        input.Gameplay.Movement.performed += ctx => moveDirection = ctx.ReadValue<float>();
        input.Gameplay.Movement.canceled += _ => MovementCancel();
    }

    private const string TabNameMovement = "Move";
    
    private static readonly int Speed = Animator.StringToHash("Speed");
}