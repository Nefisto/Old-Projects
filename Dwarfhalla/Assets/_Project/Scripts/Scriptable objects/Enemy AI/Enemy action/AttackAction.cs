using System;
using Sirenix.OdinInspector;

[HideReferenceObjectPicker]
[Serializable]
public class AttackAction : EnemyAction
{
    public BlockData targetBlock;
    public bool killTarget;

    public override void CalculatePriority()
    {
        var killPriority = killTarget ? ActionPriorityUpdate.PositiveHigh : ActionPriorityUpdate.PositiveMedium;

        UpdatePriority(killPriority);
    }
}