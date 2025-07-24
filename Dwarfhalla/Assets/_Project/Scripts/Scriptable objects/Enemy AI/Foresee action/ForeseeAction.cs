using System;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;

[Serializable]
[HideReferenceObjectPicker]
public class ForeseeAction
{
    public int priority = 1;
    public NDictionary<ActionType, List<ForeseeActionResult>> actionTypeToTargets = new();

    public virtual void CalculatePriority()
    {
        priority = actionTypeToTargets.Keys.Count * 2;

        priority += actionTypeToTargets
            .Sum(tuple => tuple.Value.Max(e => e.Priority));

        foreach (var (actionType, possibleActions) in actionTypeToTargets.ToDictionary(b => b.Key, b => b.Value))
            actionTypeToTargets[actionType] = possibleActions.OrderByDescending(e => e.Priority).ToList();
    }
}