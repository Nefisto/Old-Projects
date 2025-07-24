using System.Collections;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = EditorConstants.CARDS_PATH + "Attack")]
public class AttackCard : Card
{
    public override string Name => "Attack";

    public override bool CanBePerformed()
    {
        if (!TurnContext.TargetBlock.HasUnitOnIt)
            return false;

        if (!TurnContext.TargetBlock.UnitData.CanAttack)
            return false;
        
        // return TurnContext.TargetBlock.UnitOnBlock.UnitSide == UnitSide.Dwarf;
        return true;
    }

    public override void PreviewExecution()
    {
        CommonOperations.RunningPreviews.Add(new NTask(TurnContext.TargetBlock.UnitData.PreviewAttack()));
    }

    public override IEnumerator Perform (object context)
    {
        yield return TurnContext.TargetBlock.UnitData.PerformAttack();
    }
}