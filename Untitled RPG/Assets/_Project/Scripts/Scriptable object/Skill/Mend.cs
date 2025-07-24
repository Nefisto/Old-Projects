using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Mend",
    menuName = EditorConstants.MenuAssets.ACTIVE_SKILLS + "Mend", order = 0)]
public class Mend : SingleChargeSkill
{
    [TitleGroup("Settings")]
    [SerializeField]
    private int flatAmountOnUse;

    protected override IEnumerator BeforeCalculateActionValues (BattleActionContext context)
    {
        foreach (var actionInfo in context)
            actionInfo.flatHeal = flatAmountOnUse;

        yield break;
    }

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        foreach (var actionInfo in context)
        {
            yield return actionInfo
                .target
                .HealHealth(actionInfo);
        }
    }
}