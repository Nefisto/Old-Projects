using System;
using Sirenix.OdinInspector;
using UnityEngine;

[HideReferenceObjectPicker]
[Serializable]
public class MovementAction : EnemyAction
{
    public BlockData targetBlock;

    public int currentPositionDangerLevel;
    public int targetPositionDangerLevel;

    public Vector2Int TargetPosition => targetBlock?.Position ?? Vector2Int.zero;

    public override void CalculatePriority()
    {
        var priorityChange = (targetPositionDangerLevel
                              - currentPositionDangerLevel) switch
        {
            > 0 => ActionPriorityUpdate.NegativeMedium,
            < 0 => ActionPriorityUpdate.PositiveMedium,
            0 => ActionPriorityUpdate.Zero
        };

        UpdatePriority(priorityChange);
    }
}

[HideReferenceObjectPicker]
[Serializable]
public class ClashAction : MovementAction
{
    public bool willDie;
    public bool willKill;

    public override void CalculatePriority()
    {
        ClashDamagePriority();
        LocationDangerPriority();
        PieceWorthComparison();
    }

    private void PieceWorthComparison()
    {
        var pieceWorthComparison = (targetBlock.UnitData.UnitWorth - unitBlock.UnitData.UnitWorth) switch
        {
            0 => ActionPriorityUpdate.PositiveSmall,
            > 0 => ActionPriorityUpdate.PositiveHigh,
            < 0 => ActionPriorityUpdate.NegativeSmall
        };

        UpdatePriority(pieceWorthComparison);
    }

    private void LocationDangerPriority()
    {
        var dangerPriority = (targetPositionDangerLevel
                              - currentPositionDangerLevel) switch
        {
            > 0 => ActionPriorityUpdate.NegativeMedium,
            < 0 => ActionPriorityUpdate.PositiveMedium,
            0 => ActionPriorityUpdate.Zero
        };

        UpdatePriority(dangerPriority);
    }

    private void ClashDamagePriority()
    {
        var chance = (willDie, willKill) switch
        {
            (true, false) => ActionPriorityUpdate.NegativeMedium,
            (true, true) => targetBlock.UnitData.UnitWorth > unitBlock.UnitData.UnitWorth
                ? ActionPriorityUpdate.PositiveHigh
                : ActionPriorityUpdate.PositiveMedium,
            (false, true) => ActionPriorityUpdate.PositiveHigh,
            (false, false) => ActionPriorityUpdate.PositiveSmall
        };

        UpdatePriority(chance);
    }
}