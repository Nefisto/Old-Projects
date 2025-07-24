using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleWithSpecificAmountOfEnemies : MonoBehaviour
{
    [Title("Settings")]
    [MinValue(1), MaxValue(3)]
    [SerializeField]
    private int amountOfEnemies = 1;

    public void StartBattle()
    {
        var possibleEnemies = Database
            .Enemies
            .Data
            .ToList();

        var enemies = new List<EnemyData>();
        for (var i = 0; i < amountOfEnemies; i++)
        {
            var selectedEnemy = possibleEnemies
                .GetRandom();

            enemies.Add(selectedEnemy.GetInstance());
        }

        GameEvents.onBattleTriggered?.Invoke(new BattleSetupContext
        {
            enemiesData = enemies
        });
    }
}