using System;
using System.Collections.Generic;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class WaveGroup
{
    public List<GameObject> enemiesToSpawn;
}

public class WaveManager : MonoBehaviour
{
    [Title("Status")]
    [SerializeField]
    private RuntimeSet enemiesAlive;
    
    [Title("Respawn")]
    [SerializeField]
    private List<Transform> spawnPoints;

    [SerializeField]
    private List<WaveGroup> waves;

    private int currentEnemiesAlive => enemiesAlive.Count;
    private int currentWave = 0;

    [DisableInEditorMode]
    [Button]
    private void NextWave()
    {
        
    }

    [DisableInEditorMode]
    [Button]
    private void KillAllInCurrentWave()
    {
        for (var i = enemiesAlive.items.Count - 1; i >= 0; i--)
        {
            var currentEnemy = enemiesAlive.items[i];
            
            currentEnemy.GetComponent<Enemy>().Die();
        }
    }
}