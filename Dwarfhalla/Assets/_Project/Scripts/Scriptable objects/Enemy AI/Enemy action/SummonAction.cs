using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class SummonAction : EnemyAction
{
    public BlockData blockToSummon;
    public NDictionary<Vector2Int, ForeseeAction> positionToForeseeActions = new();
    public ICard selectedCard;

    [ReadOnly]
    private ForeseeAction selectedForeseeAction;

    public UnitData unitToSummon;

    // Called after calculate all possible positions to place the piece
    public void Setup()
    {
        var selectedEntry = positionToForeseeActions
            .OrderByDescending(t => t.Value.priority)
            .FirstOrDefault();

        blockToSummon = CommonOperations.GetBlockDataAt(selectedEntry.Key);
        selectedForeseeAction = selectedEntry.Value;

        var percentageToReduceBasedOnCost = (actionCost - 1) * .1f;
        priority = Mathf.FloorToInt((unitToSummon.UnitWorth + selectedForeseeAction.priority)
                                    * (1 - percentageToReduceBasedOnCost));
    }

    public void Sort()
    {
        positionToForeseeActions = positionToForeseeActions
            .OrderByDescending(e => e.Value.priority)
            .ToNDictionary(e => e.Key, e => e.Value);
    }
}