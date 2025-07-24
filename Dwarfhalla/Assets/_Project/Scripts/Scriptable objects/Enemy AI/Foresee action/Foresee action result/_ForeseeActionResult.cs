using System;
using Sirenix.OdinInspector;

/// <summary>
///     When doing calculations for AI, we need to check the actions without acting, so this guy fill this role
/// </summary>
[Serializable]
[HideReferenceObjectPicker]
public abstract class ForeseeActionResult
{
    public UnitData target;

    [ShowInInspector]
    public abstract int Priority { get; }
}