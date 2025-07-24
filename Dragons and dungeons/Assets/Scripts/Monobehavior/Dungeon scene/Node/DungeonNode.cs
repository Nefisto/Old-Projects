using System;
using System.Collections.Generic;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class DungeonNode : MonoBehaviour
{
    public event Action<BattleEncounterContext> OnEncounter;

    [Title("Control")]
    public DungeonNode next;
    
    [Title("Settings")]
    [SerializeField]
    private List<EnemyGroup> possibleEnemies;

    [DisableInEditorMode]
    [Button]
    public void Reached()
    {
        var encounterContext = new BattleEncounterContext
        {
            enemyGroup = RandomizeEnemyGroup()
        };
        
        OnEncounter?.Invoke(encounterContext);
    }

    public bool IsEndNode()
        => next == null;
    
    private EnemyGroup RandomizeEnemyGroup()
        => possibleEnemies.NTGetRandom();
}