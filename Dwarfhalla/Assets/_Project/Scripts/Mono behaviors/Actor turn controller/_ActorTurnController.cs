using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class ActorTurnController : MonoBehaviour
{
    public abstract IEnumerator TurnHandle();

    protected IEnumerator TriggerTurnStartOfPieces (UnitSide side)
    {
        foreach (var blockData in CommonOperations.GetAllBlocksOnCurrentRoom()
                     .Where(bd => bd.HasUnitOnIt && bd.UnitData.UnitSide == side))
            yield return blockData.UnitData.TurnStart();
        yield return CommonOperations.ProcessDeathAnimation();
    }

    protected IEnumerator TriggerTurnEndOfPieces (UnitSide side)
    {
        foreach (var blockData in CommonOperations.GetAllBlocksOnCurrentRoom()
                     .Where(bd => bd.HasUnitOnIt && bd.UnitData.UnitSide == side))
            yield return blockData.UnitData.TurnEnd();
        yield return CommonOperations.ProcessDeathAnimation();
    }
}