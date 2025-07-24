using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = EditorConstants.MenuAssets.BASIC_ACTIONS_SKILLS + "Attack",
    order = -10)]
public class BasicAttack : Skill
{
    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield return new WaitForSeconds(.2f);

        foreach (var actionInfo in context)
            yield return actionInfo.target.TakePhysicalDamage(actionInfo);
    }
}