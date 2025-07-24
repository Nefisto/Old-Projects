using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Chance to apply effect",
    menuName = EditorConstants.MenuAssets.INSTANT_CHARGE_SKILLS + "Chance to apply effect", order = 0)]
public class ChanceToApplyEffectInstant : InstantChargeAbility
{
    [TitleGroup("Settings")]
    [SerializeField]
    private StatusEffectData effectData;

    public override StatusEffectKind IconKind => StatusEffectKind.Stun;

    public override IEnumerator ApplyAbility (BattleActionContext context)
    {
        foreach (var actionInfo in context)
            actionInfo.effectInfo.Add(new EffectInfo
            {
                data = effectData,
                hasApplied = true
            });

        yield break;
    }
}