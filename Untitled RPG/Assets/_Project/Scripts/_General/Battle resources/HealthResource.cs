using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class HealthResource : GameResource
{
    [TitleGroup("Settings")]
    [SerializeField]
    private float baseRegeneration;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float accumulatedRegeneration;

    private BattleActor owner;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private List<GameResourceRegenerationModifier> regenerationModifiers = new();

    [TitleGroup("Debug")]
    [ShowInInspector]
    private float CurrentRegeneration => baseRegeneration + regenerationModifiers.Sum(x => x.flatAmount);

    public override void Setup (SetupSettings settings)
    {
        base.Setup(settings);

        var healthSettings = settings as HealthResourceSetupSettings;
        baseRegeneration = healthSettings.regenerationPerSecond;
        owner = healthSettings.owner;

        accumulatedRegeneration = 0f;
        regenerationModifiers = new();
    }

    public void Tick()
    {
        if (Current == CurrentMax)
        {
            accumulatedRegeneration = 0f;
            return;
        }

        accumulatedRegeneration += CalculateRegenerationPerSecond();
        var integerPart = (int)accumulatedRegeneration;
        accumulatedRegeneration -= integerPart;

        Current += integerPart;
    }

    public void AddRegenerationModifier (GameResourceRegenerationModifier regenerationModifier)
        => regenerationModifiers.Add(regenerationModifier);

    public void RemoveRegenerationModifier (string id) => regenerationModifiers.RemoveAll(r => r.id == id);

    private float CalculateRegenerationPerSecond()
    {
        var amountToReg = baseRegeneration + regenerationModifiers.Sum(r => r.flatAmount);
        return amountToReg * Time.deltaTime;
    }

    public class HealthResourceSetupSettings : SetupSettings
    {
        public float regenerationPerSecond;
    }
}