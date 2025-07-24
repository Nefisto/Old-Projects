using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Horizontal movement", menuName = EditorConstants.CARDS_PATH + "Horizontal movement")]
public class HorizontalMovement2 : MovementCard
{
    [TitleGroup("Debug")]
    [ShowInInspector]
    public override string Name => "Horizontal movement";

    protected override List<Vector2Int> GetPossiblePositions()
    {
        var blockData = TurnContext.TargetBlock;

        return CommonOperations
            .GetPlusPatternFrom(blockData.Position.x, blockData.Position.y, range);
    }
}