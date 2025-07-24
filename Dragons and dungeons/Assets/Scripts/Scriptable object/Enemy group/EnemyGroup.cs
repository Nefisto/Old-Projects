using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.EnemyGroupName, menuName = Nomenclature.EnemyGroupMenu, order = 0)]
public class EnemyGroup : ScriptableObject, IEnumerable<EnemyData>
{
    // TODO: Make it addressable
    [Title("Enemies")]
    public EnemyData enemyA;
    public EnemyData enemyB;
    public EnemyData enemyC;
    public EnemyData enemyD;

    public int Count => CountEnemies();

    private int CountEnemies()
    {
        var count = 0;

        count += enemyA ? 1 : 0;
        count += enemyB ? 1 : 0;
        count += enemyC ? 1 : 0;
        count += enemyD ? 1 : 0;
        
        return count;
    }

    public EnemyData this [int key]
    {
        get
        {
            if (key < 0 || key > GameConstants.Battle.MaxEnemiesInBattle)
                throw new ArgumentOutOfRangeException();

            return this.Skip(key).First();
        }
    }
    
    public IEnumerator<EnemyData> GetEnumerator()
    {
        yield return enemyA;
        yield return enemyB;
        yield return enemyC;
        yield return enemyD;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}