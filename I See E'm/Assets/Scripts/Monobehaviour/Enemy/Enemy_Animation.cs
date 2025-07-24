using DG.Tweening;
using UnityEngine;

public partial class Enemy
{
    private static readonly int SpeedX = Animator.StringToHash("Speed_X");
    private static readonly int SpeedY = Animator.StringToHash("Speed_Y");

    #region API

    public void UpdateMovementAnimation(Vector3 direction)
    {
        animator.SetFloat(SpeedX, direction.x);
        animator.SetFloat(SpeedY, direction.z);
    }

    public void UpdateLookAt(Vector3 targetPosition)
    {
        transform.DOLookAt(targetPosition, 1f / rotateSpeed);
    }
    
    #endregion
}