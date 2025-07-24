using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage increase", menuName = EditorConstants.MenuAssets.BUFF_EFFECT + "Damage increase",
    order = 0)]
public class DamageIncrease : StatusEffectData
{
    [TitleGroup("Settings")]
    [SerializeField]
    private AbilityLevel abilityLevel;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private float multiplierIncrease;

    public override StatusEffectKind Kind => StatusEffectKind.Damage;
    public override string MessageOnScreen => "DMG +";

    public override IEnumerator Setup (StatusEffectContext ctx)
    {
        yield return base.Setup(ctx);

        multiplierIncrease = Database
            .GameConstantsSo
            .GetChargeAbilityMod(Kind, abilityLevel)
            .multiplier;
    }

    public override IEnumerator ApplyOnCurrentAction (ActionInfo actionInfo)
    {
        yield return base.ApplyOnCurrentAction(actionInfo);

        actionInfo.percentageOfFlatDamageBonus.Add(("Damage increase", multiplierIncrease));
    }
}