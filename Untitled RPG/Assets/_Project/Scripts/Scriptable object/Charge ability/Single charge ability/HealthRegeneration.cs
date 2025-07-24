using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Health regeneration",
    menuName = EditorConstants.MenuAssets.SINGLE_CHARGE_SKILL + "Health regeneration", order = 0)]
public class HealthRegeneration : SingleChargeAbility
{
    [TitleGroup("Settings")]
    [MinValue(1)]
    [SerializeField]
    private int regenerationPerSecond = 5;

    public override StatusEffectKind IconKind => StatusEffectKind.LifeRegenerationIncrease;

    public override IEnumerator RegisterAbility()
    {
        ServiceLocator
            .BattleContext
            .Player
            .HealthResource
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
            .HealthResource
            .RemoveRegenerationModifier("Regeneration");

        yield break;
    }
}