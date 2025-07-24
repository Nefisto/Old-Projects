using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage modifier",
    menuName = EditorConstants.MenuAssets.PASSIVE_SKILLS + "Damage modifier")]
public class DamageModifierBasedOnType : PassiveSkill
{
    [TitleGroup("Settings")]
    [SerializeField]
    private int amountToIncrease;

    [TitleGroup("Settings")]
    [SerializeField]
    private MainTraitKind mainTraitKind;

    private void Validate (BattleActionContext ctx)
    {
        if (ctx.caster != owner)
            return;

        if (ctx.caster.ActorData.MainTraitKind != mainTraitKind)
            return;

        ctx.skill.GettingActionInfoEntryPoint += Apply;
    }

    private IEnumerator Apply (BattleActionContext ctx)
    {
        foreach (var actionInfo in ctx.Actions)
            actionInfo.flatExtraDamage.Add(new DamageModifierPassiveInfo(Name, amountToIncrease));

        yield return null;
    }

    public override IEnumerator Register()
    {
        TurnController.OnRunningAction += Validate;
        yield break;
    }

    public override IEnumerator Remove()
    {
        TurnController.OnRunningAction -= Validate;
        yield break;
    }
}