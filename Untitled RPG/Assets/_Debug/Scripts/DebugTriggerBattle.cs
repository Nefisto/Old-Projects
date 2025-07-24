using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugTriggerBattle : SerializedMonoBehaviour
{
    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Dictionary<string, Location> nameToLocation;

    private void Awake()
    {
        nameToLocation = FindObjectsByType<Location>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .OrderBy(l => l.LocationLevel)
            .ToDictionary(l => l.name);
    }

    public void TriggerCurrentLocationBattle()
    {
        var ctx = ServiceLocator
            .LocationDetector
            .GetLocationContext();

        var enemies = ctx.GetEnemies();
        var modifiers = new List<LocationModifier> { ctx.locationModifier };
        TriggerBattle(enemies, modifiers);
    }

    private static void TriggerBattle (List<EnemyData> enemies, List<LocationModifier> modifiers)
        => GameEvents.onBattleTriggered?.Invoke(new BattleSetupContext
        {
            enemiesData = enemies,
            locationModifiers = modifiers
        });
}