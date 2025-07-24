using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class TickResource : GameResource
{
    [field: MinValue(1)]
    [field: SerializeField]
    public int NotifyAfterNTicks { get; private set; } = 1;

    [field: ReadOnly]
    [field: SerializeField]
    public int AmountOfTicks { get; private set; }

    public Func<int> atbGrowth;

    [ShowInInspector]
    public int TickGrowth => atbGrowth?.Invoke() ?? 0;

    [ShowInInspector]
    [ReadOnly]
    public override int Current
    {
        get => current;
        set
        {
            var oldValue = current;
            var clampedNewValue = Mathf.Min(CurrentMax, value);
            current = clampedNewValue;

            RaiseUpdateCurrent(oldValue, current);

            if (current == currentMax)
                OnReachMax?.Invoke();
        }
    }

    [InfoBox("This max comes from Game Constants")]
    [ReadOnly]
    [PropertyOrder(-1)]
    [ShowInInspector]
    public override int DefaultMax
    {
        get => GameConstants.DEFAULT_MAX_ATB;
        protected set => base.DefaultMax = value;
    }

    public override void Setup (SetupSettings settings)
    {
        base.Setup(settings);

        OnReachMax = null;
        var tickSettings = settings as TickResourceSetupSettings;
        atbGrowth = tickSettings.ATBGrownth;
    }

    public void Tick()
    {
        var growthAmount = (atbGrowth?.Invoke() ?? 5);
        Current += Current < 0
            ? Mathf.CeilToInt(growthAmount * .5f)
            : growthAmount;

        if (++AmountOfTicks < NotifyAfterNTicks)
            return;

        OnTickNotify?.Invoke();
        AmountOfTicks = 0;
    }

    public override void Reset()
    {
        base.Reset();
        NotifyAfterNTicks = Mathf.Max(a: NotifyAfterNTicks, b: 1);
        AmountOfTicks = 0;
        Current = 0;
    }

    public event Action OnReachMax;
    public event Action OnTickNotify;

    public class TickResourceSetupSettings : SetupSettings
    {
        public Func<int> ATBGrownth;
    }
}