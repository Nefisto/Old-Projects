using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerBattleManually : MonoBehaviour
{
    public List<LocationModifier> locationModifier;
    public List<EnemyDataFactory> enemiesToBattle;

    [Button]
    [DisableInEditorMode]
    private void TriggerBattle()
    {
        var battleContext = new BattleSetupContext
        {
            enemiesData = enemiesToBattle.Select(e => e.GetInstance()).ToList(),
            locationModifiers = locationModifier
        };

        GameEvents.onBattleTriggered?.Invoke(battleContext);
    }
}