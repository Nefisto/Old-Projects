using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Fireball is thrown by machine state, this script will give utilities only
/// </summary>
public partial class Player
{
    public event Action OnSuccessThrownFireball;
    public event Action OnFailedThrownFireball;

    [FoldoutGroup(FO_Skills, false)]
    [TabGroup(FO_TAB_Skills, TabName_Fireball)]
    [Title("Status")]
    [PropertyRange(0f, "$maxFuelAmount")]
    [SerializeField]
    private float fireballCost = 20f;

    [TabGroup(FO_TAB_Skills, TabName_Fireball)]
    [Tooltip("Time to recovery before be able to cast another fireball")]
    [SerializeField]
    private float fireballRecoveryTime = 1f;

    [TabGroup(FO_TAB_Skills, TabName_Fireball)]
    [Required]
    [SerializeField]
    private GameObject fireballPrefab;

    [TabGroup(FO_TAB_Skills, TabName_Fireball)]
    [SerializeField]
    private Transform fireballFolder;

    #region Debug

    [TabGroup(FO_TAB_Skills, TabName_Fireball)]
    [Title("Debug")]
    [ReadOnly]
    [ProgressBar(0f, 1f, ColorGetter = "GetFireballRecoveryColor", DrawValueLabel = false)]
    [SerializeField]
    private float fireballRecovery = 1f;

    private bool IsRecovering => !fireballRecovery.IsNearlyEnoughTo(1f);

    #endregion

    #region API

    public void CastFireballAtTarget (Enemy target)
    {
        var instance = Instantiate(fireballPrefab, FlashlightPosition.position, FlashlightPosition.rotation, fireballFolder)
            .GetComponent<Fireball>();

        instance.Setup(target);
    }

    public void CastFireballAtDirection (Vector3 dir)
    {
        var instance = Instantiate(fireballPrefab, FlashlightPosition.position, FlashlightPosition.rotation, fireballFolder)
            .GetComponent<Fireball>();

        instance.Setup(dir);
    }

    #endregion

    #region Private

    private void RegisterFireballInput()
    {
        var input = InputController.Input;

        input.Gameplay.CastFireball.performed += _ =>
        {
            if (!CanCastFireball())
            {
                OnFailedThrownFireball?.Invoke();
                return;
            }

            OnSuccessThrownFireball?.Invoke();

            stateMachine.CurrentState.CastFireball();
        };
    }

    private void RegisterFireballListeners()
    {
        OnSuccessThrownFireball += () =>
        {
            UpdateFuel(-fireballCost);
            StartCoroutine(RecoveryFireball());
        };
    }

    private bool CanCastFireball()
    {
        if (isCastingRay)
            return false;

        if (IsRecovering)
            return false;

        if (!HaveEnoughFuel(fireballCost))
            return false;

        return true;
    }

    private IEnumerator RecoveryFireball()
    {
        yield return new NTWaitForSecondsActioningUntil(fireballRecoveryTime,
            timer => fireballRecovery = Mathf.Clamp(timer / fireballRecoveryTime, 0f, 1f));
    }

    #endregion

    #region Debug

    // ReSharper disable once UnusedMember.Local
    private Color GetFireballRecoveryColor()
        => fireballRecovery.IsNearlyEnoughTo(1f) ? Color.green : Color.yellow;

    #endregion
}