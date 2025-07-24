using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Experience multiplier",
    menuName = EditorConstants.MenuAssets.LOCATION_MODIFIERS + "Experience multiplier")]
public class ExperienceMultiplier : LocationModifier
{
    [field: TitleGroup("Settings")]
    [field: Range(-3, 3f)]
    [field: SerializeField]
    public float MultiplierIncrease { get; private set; }

    [TitleGroup("Settings")]
    [ShowInInspector]
    public override string NameShowOnField
        => $"Experience {(MultiplierIncrease > 0 ? "increase" : "decrease")}: {1 + MultiplierIncrease}x";

    public override IEnumerator Register()
    {
        BattleManager.setupBattleContextEntryPoint += AddExperienceMultiplier;
        yield break;
    }

    public override IEnumerator Remove()
    {
        yield break;
    }

    private IEnumerator AddExperienceMultiplier (IEntryPointContext arg)
    {
        ServiceLocator.BattleResulData.AddMultiplier(this);
        yield break;
    }
}