using System;
using UnityEngine;

public abstract class Modifier : IEquatable<Modifier>
{
    // Modifier Icon
    public virtual Sprite Icon => null;

    /// <summary>
    /// This will run when the buff is applied
    /// </summary>
    public abstract void Apply (ModifierContext modifierContext);
    
    // Answer to start and end events
    public virtual void StartTurnEffect(ModifierContext modifierContext) {}
    public virtual void EndTurnEffect(ModifierContext modifierContext) {}

    /// <summary>
    /// Run when buff was removed
    /// </summary>
    public abstract void Remove (ModifierContext modifierContext);
    
    /// <summary>
    /// When the same modifier is applied to a target
    /// </summary>
    public virtual void RefreshModifier () {}

    /// <summary>
    /// To allow user to properly behave when modifiers were equal
    /// </summary>
    public virtual bool Equals (Modifier other)
    {
        if (other == null)
            return false;
        
        return GetType() == other.GetType();
    }
}