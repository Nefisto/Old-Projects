using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class EncounterRateSystem : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private GradientBar gradientBar;

    /// <summary>
    ///     Encounter percentage will used to evaluate this curve, and will provide a chance for a battle to happen
    /// </summary>
    [TitleGroup("Settings")]
    [SerializeField]
    private AnimationCurve triggerBattleProbability;

    [TitleGroup("Settings")]
    [SerializeField]
    private TickResource tickResource;

    public float MinimumValue => triggerBattleProbability.keys[1].time;

    private void Awake()
    {
        tickResource.OnUpdatedCurrent += (_, _) =>
        {
            OnEncounterRateUpdate?.Invoke(new EncounterRateUpdateContext
            {
                resourcePercentage = tickResource.CurrentPercentage,
                battleTriggerPercentage = triggerBattleProbability.Evaluate(tickResource.CurrentPercentage)
            });

            gradientBar.UpdateBar(tickResource.CurrentPercentage);
        };

        GameEvents.OnTickEncounter += tickResource.Tick;
        GameEvents.OnBattleFinishedEntryPoint += _ => tickResource.Reset();

        tickResource.OnTickNotify += BattleCheck;
    }

    private void Start() => tickResource.Reset();

    public static event Action<EncounterRateUpdateContext> OnEncounterRateUpdate;

    private void BattleCheck()
    {
        var chanceForBattle = triggerBattleProbability.Evaluate(tickResource.CurrentPercentage);
        if (Random.value < chanceForBattle)
            TriggerBattle();
    }

    private void TriggerBattle()
    {
        var ctx = ServiceLocator
            .LocationDetector
            .GetLocationContext();

        var enemies = ctx.GetEnemies();
        var modifiers = new List<LocationModifier> { ctx.locationModifier };
        var battleContext = new BattleSetupContext
        {
            enemiesData = enemies,
            locationModifiers = modifiers
        };

        GameEvents.onBattleTriggered?.Invoke(battleContext);
    }
}