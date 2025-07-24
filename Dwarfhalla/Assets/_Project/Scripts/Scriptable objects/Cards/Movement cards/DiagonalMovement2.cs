using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Diagonal movement", menuName = EditorConstants.CARDS_PATH + "Diagonal movement")]
public class DiagonalMovement2 : MovementCard
{
    [ShowInInspector]
    public override string Name => "Diagonal Movement";

    protected override List<Vector2Int> GetPossiblePositions()
        => CommonOperations
            .GetCrossPatternFrom(TurnContext.TargetBlock.Position.x, TurnContext.TargetBlock.Position.y, range);
}