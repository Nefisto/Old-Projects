using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage increase",
    menuName = EditorConstants.MenuAssets.PERSISTENT_CHARGE_SKILLS + "Damage increase", order = 0)]
public class PersistentDamageIncrease : PersistentChargeAbility
{
    public override StatusEffectKind IconKind => StatusEffectKind.Damage;

    public override IEnumerator ApplyAbility (BattleActionContext context)
    {
        foreach (var actionInfo in context)
        {
            var target = statusEffect.EffectTarget == StatusEffectData.StatusEffectTarget.Target
                ? actionInfo.target
                : actionInfo.caster;

            yield return target.StatusEffectController.ApplyStatusEffect(new List<StatusEffectData> { statusEffect });
        }
    }
}