using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Player
{
    public static event Action<float> OnUpdateFuel; // Update with a percentage value
    
    [TabGroup(TabNameFuel, true)]
    [Title("Status")]
    [SerializeField]
    private float maxFuelAmount = 100f;

    [TabGroup(TabNameFuel)]
    [SerializeField]
    private float fuelRecoveryPerSecond = 5f;

    [TabGroup(TabNameFuel)]
    [Tooltip("In which percentage the bar will begin to recharge?")]
    [Range(0, 1f)]
    [SerializeField]
    private float rechargeThreshold = .33f;

    [TabGroup(TabNameFuel)]
    [Title("Settings")]
    [Tooltip("Time to recheck if fuel bar can be recharged")]
    [SerializeField]
    private float timeBetweenChecks = .5f;
    
    #region Debug

#if UNITY_EDITOR
    [TabGroup(TabNameFuel)]
    [Title("Debug")]
    [Tooltip("Pause/Unpause fuel recharge task when changed on inspector(only)")]
    [SerializeField]
    private bool enableFuel = true;
#endif
    
    [TabGroup(TabNameFuel)]
    [ReadOnly]
    [SerializeField]
    private float currentFuelAmount;
    
    [TabGroup(TabNameFuel)]
    [DisableInEditorMode]
    [InlineButton("UpdateFuel")]
    [SerializeField]
    private float refuelAmount;

    #endregion

    #region Properties

    private float CurrentFuelPercentage => currentFuelAmount / maxFuelAmount; 

    #endregion

    private Task updateFuelAmountRoutine;

    #region API

    public void UpdateFuel (float amount)
    {
        currentFuelAmount = Mathf.Clamp(currentFuelAmount + amount, 0f, maxFuelAmount);
        
        OnUpdateFuel?.Invoke(currentFuelAmount / maxFuelAmount);

        if (updateFuelAmountRoutine.Paused)
        {
            updateFuelAmountRoutine.Unpause();
        }
    }

    #endregion

    #region Private methods

    private void SetupFuelUpdateTask()
    {
        updateFuelAmountRoutine = new Task(UpdateFuel());

        // SHARP: This can be properly closed and restarted when conditions to run didn't met
        IEnumerator UpdateFuel()
        {
            while (true)
            {
#if UNITY_EDITOR
                if (!enableFuel)
                {
                    yield return new WaitForSeconds(timeBetweenChecks);
                    continue;
                }
#endif
                // Check if can recharge
                if (currentFuelAmount.IsNearlyEnoughTo(maxFuelAmount) // Full
                    || CurrentFuelPercentage <= rechargeThreshold) // Lower than threshold
                {
                    yield return new WaitForSeconds(timeBetweenChecks);
                    continue;
                }

                var deltaRecovery = fuelRecoveryPerSecond * Time.deltaTime;
                currentFuelAmount = Mathf.Clamp(currentFuelAmount + deltaRecovery, 0, maxFuelAmount);
                OnUpdateFuel?.Invoke(currentFuelAmount / maxFuelAmount);

                yield return null;
            }
        }
    }

    private bool HaveEnoughFuel (float amount)
        => currentFuelAmount >= amount;
    
    private bool TrySpendFuel (float amount)
    {
        if (currentFuelAmount < amount)
            return false;

        UpdateFuel(-amount);

        return true;
    }

    #endregion

    #region Debug

    [TabGroup(TabNameFuel)]
    [DisableInEditorMode]
    [Button]
    private void ResetFuel()
    {
        UpdateFuel(maxFuelAmount);
    }

    #endregion
    
    private const string TabNameFuel = "Fuel";
}