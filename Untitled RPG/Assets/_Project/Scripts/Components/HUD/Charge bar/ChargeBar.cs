using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public enum ChargeModeEnum
{
    LevelCharge,
    SingleCharge
}

public class ChargeBar : MonoBehaviour
{
    [FormerlySerializedAs("levelMode")]
    [TitleGroup("References")]
    [SerializeField]
    private ChargeLevelMode levelChargeMode;

    [TitleGroup("References")]
    [SerializeField]
    private SingleChargeMode singleChargeMode;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private ChargeMode currentUsedMode;

    private void Awake()
    {
        ServiceLocator.ChargeBar = this;
        GameEvents.onBattleTriggered += _ => Close();
        GameEvents.onFinishedLoadingData += () =>
        {
            levelChargeMode.gameObject.SetActive(false);
            singleChargeMode.gameObject.SetActive(false);
        };
    }

    public IEnumerator Setup (ChargeModeEnum mode, ChargeMode.Settings settings)
    {
        currentUsedMode = GetCorrectChargeMode(mode);
        yield return currentUsedMode.Setup(settings);
        currentUsedMode.gameObject.SetActive(true);
    }

    public void Close()
    {
        if (currentUsedMode == null)
            return;

        currentUsedMode.gameObject.SetActive(false);
    }

    private ChargeMode GetCorrectChargeMode (ChargeModeEnum chargeModeEnum)
        => chargeModeEnum switch
        {
            ChargeModeEnum.LevelCharge => levelChargeMode,
            ChargeModeEnum.SingleCharge => singleChargeMode,
            _ => throw new ArgumentOutOfRangeException(nameof(chargeModeEnum), chargeModeEnum, null)
        };
}