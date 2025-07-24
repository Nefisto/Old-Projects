using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Player
{
    [TabGroup(TabNameRotate)]
    [Title("Status")]
    [SerializeField]
    private float rotateSpeed = 10f;

    [TabGroup(TabNameRotate)]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool canRotate = true;
    
    [TabGroup(TabNameRotate)]
    [ReadOnly]
    [SerializeField]
    private float rotateDirection;

    #region API

    public void DisableRotation()
        => canRotate = false;

    public void EnableRotation()
        => canRotate = true;

    #endregion

    private void Rotate()
    {
        if (!canRotate)
            return;
        
        if (rotateDirection.IsNearlyEnoughTo(0f))
            return;

        var deltaRotation = Quaternion.Euler(new Vector3(0f, rotateDirection * rotateSpeed, 0f) * Time.fixedDeltaTime);
        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
    }

    private void RegisterRotateInputs()
    {
        var input = InputController.Input;

        input.Gameplay.Rotate.performed += ctx => rotateDirection = ctx.ReadValue<float>();
        input.Gameplay.Rotate.canceled += _ => rotateDirection = 0f;
    }

    private const string TabNameRotate = "Rotate";
}