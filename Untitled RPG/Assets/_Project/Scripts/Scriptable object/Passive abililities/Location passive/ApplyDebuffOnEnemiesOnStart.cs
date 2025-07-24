using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Apply debuff on enemies",
    menuName = EditorConstants.MenuAssets.LOCATION_MODIFIERS + "Apply debuff on enemies")]
public class ApplyDebuffOnEnemiesOnStart : LocationModifier
{
    [TitleGroup("Settings")]
    [SerializeField]
    private StatusEffectData statusEffect;

    public override string NameShowOnField => $"Apply begin with {statusEffect.name}";

    public override IEnumerator Register()
    {
        BattleManager.battleStartingEntryPoint += ApplyingDebuff;
        yield break;
    }

    public override IEnumerator Remove()
    {
        yield break;
    }

    private IEnumerator ApplyingDebuff()
    {
        var enemies = ServiceLocator.BattleContext.Enemies;

        foreach (var battleActor in enemies)
            yield return battleActor.StatusEffectController.ApplyStatusEffect(
                new List<StatusEffectData> { statusEffect });
    }
}