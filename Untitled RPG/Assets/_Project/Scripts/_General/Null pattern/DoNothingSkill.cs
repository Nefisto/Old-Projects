using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Nothing", menuName = EditorConstants.MenuAssets.ACTIVE_SKILLS + "Do nothing", order = 0)]
public class DoNothingSkill : Skill
{
    public override IEnumerator Run (BattleActionContext context)
    {
        yield break;
    }

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield break;
    }
}