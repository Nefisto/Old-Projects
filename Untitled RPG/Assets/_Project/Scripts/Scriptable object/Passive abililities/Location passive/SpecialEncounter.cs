using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Special encounter",
    menuName = EditorConstants.MenuAssets.LOCATION_MODIFIERS + "Special encounter")]
public class SpecialEncounter : LocationModifier
{
    [TitleGroup("Settings")]
    [SerializeField]
    private string encounterName;

    [TitleGroup("Settings")]
    [SerializeField]
    private List<EnemyDataFactory> enemies;

    [TitleGroup("Debug")]
    [ShowInInspector]
    public override string NameShowOnField => $"Special encounter ({encounterName})";

    public override IEnumerator Register()
    {
        BattleManager.setupingEnemiesEntryPoint += UpdateEnemiesInBattle;
        yield break;
    }

    public override IEnumerator Remove()
    {
        BattleManager.setupingEnemiesEntryPoint -= UpdateEnemiesInBattle;
        yield break;
    }

    private IEnumerator UpdateEnemiesInBattle (BattleSetupContext ctx)
    {
        ctx.enemiesData = enemies
            .Select(f => f.GetInstance())
            .ToList();
        yield break;
    }
}