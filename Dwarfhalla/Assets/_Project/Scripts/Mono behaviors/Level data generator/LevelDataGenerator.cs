using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

public class LevelDataGenerator : MonoBehaviour
{
    public UnitData enemyToSpawn;
    private List<RoomData> generatedData;

    private void Start()
    {
        GameEntryPoints.GeneratingSessionData += GenerateLevelData;
    }

    private IEnumerator GenerateLevelData (object _)
    {
        yield return GenerateRoomsData();

        ServiceLocator.GameContext.LevelData = new LevelData { Rooms = generatedData };
        AddEnemies();
    }

    private void AddEnemies()
    {
        var blocksToAddEnemies = CommonOperations
            .GetAllBlocksOnCurrentRoom()
            .Shuffle()
            .Take(Random.Range(1, 1))
            .ToList();

        foreach (var blockData in blocksToAddEnemies)
        {
            blockData.InitialUnitOnThisBlock = enemyToSpawn.GetInstance;
        }
    }

    private IEnumerator GenerateRoomsData()
    {
        generatedData = new List<RoomData> { new(GameConstants.GRID_WIDTH, GameConstants.GRID_HEIGHT) };
        
        yield break;
    }
}