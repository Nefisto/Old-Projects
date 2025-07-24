using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Mana regeneration",
    menuName = EditorConstants.MenuAssets.SINGLE_CHARGE_SKILL + "Mana regeneration", order = 0)]
public class ManaRegeneration : SingleChargeAbility
{
    [TitleGroup("Settings")]
    [MinValue(1)]
    [SerializeField]
    private int regenerationPerSecond = 5;

    public override StatusEffectKind IconKind => StatusEffectKind.ManaRegenerationIncrease;

    public override IEnumerator RegisterAbility()
    {
        ServiceLocator
            .BattleContext
            .Player
            .ManaResource
            .AddRegenerationModifier(new GameResourceRegenerationModifier
            {
                id = "Regeneration",
                flatAmount = regenerationPerSecond
            });

        yield break;
    }

    public override IEnumerator RemoveAbility()
    {
        ServiceLocator
            .BattleContext
            .Player
            .ManaResource
            .RemoveRegenerationModifier("Regeneration");

        yield break;
    }
}