using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = EditorConstants.MenuAssets.JOB_SKILLS + "Test", order = 0)]
public class SpecialSkill : JobSkill
{
    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield return new WaitForSeconds(.2f);

        foreach (var actionInfo in context)
            yield return actionInfo.target.TakePhysicalDamage(actionInfo);

        yield return ApplyEffect(context);
    }
}