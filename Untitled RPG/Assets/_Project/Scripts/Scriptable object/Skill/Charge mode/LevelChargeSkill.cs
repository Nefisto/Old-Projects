using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public interface ILevelCharge
{
    public event Action OnFullCharged;
}

public interface IHoldableSkill { }

public class MultipleLevelChargeBehavior : ScriptableObject { }

public abstract class LevelChargeSkill : Skill, IHoldableSkill, ILevelCharge
{
    [TabGroup("Tab", "Charge")]
    [TitleGroup("Tab/Charge/Settings", order: 5)]
    [Tooltip("A single message that appear on bottle of the bar")]
    [SerializeField]
    private string singleMessage;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private int holdPoints;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int MaxHoldPoints => ChargePointsSettings.Sum(l => l.pointsToLevelUp);

    public bool HasCharge => holdPoints > 0;

    protected int GetChargeLevel
    {
        get
        {
            var index = 0;
            var remainingPoints = holdPoints;
            while (ChargePointsSettings.Count > 1
                   && remainingPoints >= ChargePointsSettings[index].pointsToLevelUp)
            {
                remainingPoints -= ChargePointsSettings[index].pointsToLevelUp;
                index++;

                if (index == ChargePointsSettings.Count)
                    break;
            }

            return index + 1;
        }
    }

    public (int level, float percentage) CurrentIndexAndPercentage
    {
        get
        {
            var index = 0;
            var remainingPoints = holdPoints;
            while (ChargePointsSettings.Count > 1 // a single level charge is a edge case 
                   && remainingPoints >= ChargePointsSettings[index].pointsToLevelUp)
            {
                remainingPoints -= ChargePointsSettings[index].pointsToLevelUp;
                index++;

                if (index >= ChargePointsSettings.Count - 1)
                    break;
            }

            return (index, (float)remainingPoints / ChargePointsSettings[index].pointsToLevelUp);
        }
    }

    public int HoldPoints
    {
        get => holdPoints;
        set
        {
            holdPoints = value;

            OnUpdateHoldPoints?.Invoke();
        }
    }

    public event Action OnFullCharged;

    public event Action OnUpdateHoldPoints;

    public override IEnumerator HoldBegin()
    {
        yield return ServiceLocator.ChargeBar.Setup(ChargeModeEnum.LevelCharge,
            new ChargeLevelMode.Settings
            {
                skill = this,
                singleMessage = singleMessage
            });
    }

    public override void HoldStay (HoldSettings settings)
    {
        HoldPoints = Mathf.Min(HoldPoints + Mathf.RoundToInt(settings.holdPower * .2f), MaxHoldPoints);

        if (HoldPoints >= MaxHoldPoints)
            OnFullCharged?.Invoke();
    }

    public override IEnumerator HoldFinish (HoldFinishSettings settings)
    {
        if (ServiceLocator.ChargeBar == null)
            yield break;

        if (settings.shouldHideBar)
            ServiceLocator.ChargeBar.Close();

        if (settings.shouldResetPoints)
            HoldPoints = 0;
    }

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield return base.Behavior(context);

        yield return HoldFinish(new HoldFinishSettings { shouldHideBar = true });
    }

    protected override IEnumerator BeforeCalculateActionValues (BattleActionContext context)
    {
        if (!HasCharge)
            yield break;

        foreach (var chargeAbility in GetChargeAbilities(GetChargeLevel))
            yield return chargeAbility.ApplyAbility(context);
    }

    private List<LevelChargeAbility> GetChargeAbilities (int level)
    {
        if (level == 1)
            return new List<LevelChargeAbility>();

        var abilityTypeToInstance =
            new Dictionary<LevelChargeAbility, LevelChargeAbility>(new ChargeAbility.ChargeAbilityComparer());
        foreach (var levelChargeAbility in ChargePointsSettings.Take(level - 1)
                     .SelectMany(x => x.chargeAbilities)
                     .Cast<LevelChargeAbility>())
        {
            if (!abilityTypeToInstance.ContainsKey(levelChargeAbility))
            {
                abilityTypeToInstance.Add(levelChargeAbility, levelChargeAbility);
                continue;
            }

            if (abilityTypeToInstance[levelChargeAbility].AbilityLevel < levelChargeAbility.AbilityLevel)
                abilityTypeToInstance[levelChargeAbility] = levelChargeAbility;
        }

        return abilityTypeToInstance.Values.ToList();
    }
}