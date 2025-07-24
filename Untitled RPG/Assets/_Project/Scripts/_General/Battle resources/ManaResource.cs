using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameResourceRegenerationModifier
{
    public float flatAmount;
    public string id;
}

[Serializable]
public class ManaResource : GameResource
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

        var manaSettings = settings as ManaResourceSetupSettings;
        baseRegeneration = manaSettings.regenerationPerSecond;
        owner = manaSettings.owner;

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

    public class ManaResourceSetupSettings : SetupSettings
    {
        public float regenerationPerSecond;
    }
}