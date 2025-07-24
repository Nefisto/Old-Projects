using System;

public enum ActionPriorityUpdate
{
    NegativeSmall,
    NegativeMedium,
    NegativeHigh,
    Zero,
    PositiveSmall,
    PositiveMedium,
    PositiveHigh
}

public abstract class EnemyAction
{
    public int actionCost = 1;
    public string description;
    public int priority = 1;

    public BlockData unitBlock;

    public EnemyAction (string description = null) => this.description = description;

    public virtual void CalculatePriority() => priority = 1;

    public void UpdatePriority (ActionPriorityUpdate priorityUpdate)
        => priority += priorityUpdate switch
        {
            ActionPriorityUpdate.NegativeSmall => -1,
            ActionPriorityUpdate.NegativeMedium => -3,
            ActionPriorityUpdate.NegativeHigh => -5,
            ActionPriorityUpdate.Zero => 0,
            ActionPriorityUpdate.PositiveSmall => 1,
            ActionPriorityUpdate.PositiveMedium => 3,
            ActionPriorityUpdate.PositiveHigh => 5,
            _ => throw new ArgumentOutOfRangeException(nameof(priorityUpdate), priorityUpdate, null)
        };
}