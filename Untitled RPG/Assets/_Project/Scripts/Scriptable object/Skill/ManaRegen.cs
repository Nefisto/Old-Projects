using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Mana regen",
    menuName = EditorConstants.MenuAssets.ACTIVE_SKILLS + "Mana regen", order = 0)]
public class ManaRegen : SingleChargeSkill
{
    [TitleGroup("Settings")]
    [SerializeField]
    private int flatAmountOfManaOnUse = 20;

    protected override IEnumerator Behavior (BattleActionContext context)
    {
        yield return context
            .caster
            .HealMana(flatAmountOfManaOnUse);
    }
}