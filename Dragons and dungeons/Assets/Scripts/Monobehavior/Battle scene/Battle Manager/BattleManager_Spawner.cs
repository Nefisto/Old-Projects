using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class BattleManager
{
    [TabGroup("Spawner")]
    [Title("Control")]
    [SerializeField]
    private GameObject enemyPrefab;

    [TabGroup("Spawner")]
    [SerializeField]
    private Transform enemyFolder;
    
    [TabGroup("Spawner")]
    [SerializeField]
    private List<Transform> enemySpawnPosition;
    
    [TabGroup("Spawner")]
    [Space]
    [SerializeField]
    private GameObject playerPrefab;

    [TabGroup("Spawner")]
    [SerializeField]
    private Transform playerFolder;

    [TabGroup("Spawner")]
    [SerializeField]
    private List<Transform> playerCharacterSpawnPosition;

    private void SpawnPlayerCharacters (Player player)
    {
        ClearOldPlayerCharacters();

        SpawnNewPlayerCharacters(player);
    }

    private void SpawnNewPlayerCharacters (Player player)
    {
        for (var i = 0; i < GameConstants.Battle.InitialPlayableCharactersInBattle; i++)
        {
            var currentPlayerStatus = player.GetPlayableCharacterAtIndex(i);
            var currentPlayerSpawnPosition = playerCharacterSpawnPosition[i];

            if (IsEmptySpace(currentPlayerStatus))
                continue;

            var friendlyCharacterInstance = Instantiate(playerPrefab, currentPlayerSpawnPosition.position, Quaternion.identity, playerFolder)
                .GetComponent<FriendlyBattleActor>();
            
            friendlyCharacterInstance.Setup(currentPlayerStatus);
        }
    }

    private void ClearOldPlayerCharacters()
    {
        var oldCharacters = GetPlayerCharacters();

        foreach (var oldCharacter in oldCharacters)
        {
            Destroy(oldCharacter.gameObject);
        }
    }

    private void SpawnEnemies(EnemyGroup enemyGroup)
    {
        ClearOldEnemies();

        SpawnNewEnemies(enemyGroup);
    }

    private void ClearOldEnemies()
    {
        var oldEnemies = GetEnemiesCharacters();

        for (var i = oldEnemies.Count - 1; i >= 0; i--)
            Destroy(oldEnemies[i].gameObject);
    }

    private void SpawnNewEnemies(EnemyGroup enemyGroup)
    {
        for(var i = 0; i < GameConstants.Battle.MaxEnemiesInBattle; i++)
        {
            var currentEnemyStatus = enemyGroup[i];
            var currentEnemySpawnPosition = enemySpawnPosition[i];

            if (IsEmptySpace(currentEnemyStatus))
                continue;

            var enemyInstance = Instantiate(enemyPrefab, currentEnemySpawnPosition.position, Quaternion.identity, enemyFolder)
                .GetComponent<EnemyBattleActor>();
            
            enemyInstance.Setup(currentEnemyStatus);
        }
    }

    private static bool IsEmptySpace(Object enemyA)
        => enemyA == null;
}