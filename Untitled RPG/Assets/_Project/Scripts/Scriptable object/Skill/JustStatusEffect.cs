using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Status effect", menuName = EditorConstants.MenuAssets.ACTIVE_SKILLS + "Status effect",
    order = 0)]
public class JustStatusEffect : Skill
{
    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield break;
    }
}