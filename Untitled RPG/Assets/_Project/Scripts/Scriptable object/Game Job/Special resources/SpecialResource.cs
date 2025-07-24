using System;
using System.Collections;
using UnityEngine;

public abstract partial class SpecialResource : ScriptableObject, IGameResource
{
    public GameResource gameResource;
    public virtual SpecialResource GetInstance => Instantiate(this);

    public event Action<int, int> OnUpdatedCurrent;

    public virtual int Current { get; set; }

    public abstract IEnumerator Setup (SetupSettings settings);

    protected void RaiseUpdatedCurrent (int oldValue, int newValue) => OnUpdatedCurrent?.Invoke(oldValue, newValue);

    public class SetupSettings
    {
        public GradientBar gradientBar;
    }
}