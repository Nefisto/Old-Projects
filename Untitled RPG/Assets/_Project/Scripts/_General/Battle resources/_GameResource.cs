using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class GameResource : IGameResource
{
    protected int current;
    protected int currentMax;

    [ShowInInspector]
    public float CurrentPercentage
        => Current >= 0
            ? (float)Current / CurrentMax
            : (float)Current / GameConstants.MAX_FATIGUE_AMOUNT;

    public virtual int DefaultMax { get; protected set; } = 100;

    [ShowInInspector]
    [ReadOnly]
    public int CurrentMax
    {
        get => Mathf.Max(currentMax, 1);
        set => currentMax = value;
    }

    [ShowInInspector]
    [ReadOnly]
    public virtual int Current
    {
        get => current;
        set
        {
            if (value == current)
                return;

            var oldValue = current;
            var clampedNewValue = Mathf.Clamp(value: value, min: 0, max: DefaultMax);
            current = clampedNewValue;

            OnUpdatedCurrent?.Invoke(arg1: oldValue, arg2: current);
        }
    }

    public event Action<int, int> OnUpdatedCurrent;

    protected void RaiseUpdateCurrent (int oldValue, int newValue) => OnUpdatedCurrent?.Invoke(oldValue, newValue);

    public virtual void Setup (SetupSettings settings)
    {
        OnUpdatedCurrent = null;
        CurrentMax = DefaultMax = settings.initialMax;
        Current = Mathf.RoundToInt(settings.initialMax * settings.initialPercentage);
    }

    public virtual void Reset()
    {
        CurrentMax = DefaultMax;
    }

    public void SetToPercentage (float percentage) => Current = Mathf.RoundToInt(CurrentMax * percentage);

    [PropertyOrder(10)]
    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    private void UpdateCurrent (int amount) => Current += amount;

    [PropertyOrder(10)]
    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    private void UpdateCurrentMax (int amount) => CurrentMax += amount;

    public class SetupSettings
    {
        public int initialMax;
        public float initialPercentage;
        public BattleActor owner;
    }
}