using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Escape battle",
    menuName = EditorConstants.MenuAssets.ENEMY_SKILLS + "Escape battle", order = 0)]
public class EscapeBattle : Skill
{
    protected override IEnumerator Behavior (BattleActionContext context)
    {
        var caster = context.caster as EnemyBattleActor;

        foreach (var actionInfo in context)
        {
            if (!(Random.value > actionInfo.chanceToHit))
                continue;

            ServiceLocator.FloatText.MissText(caster.transform);
            yield return new WaitForSeconds(1.5f);
            yield break;
        }

        ServiceLocator.FloatText.AddCustomFloatText(new FloatTextSettings($"-Escaped-", caster.transform,
            textColor: Color.white));
        yield return caster.LeaveBattle();
    }
}