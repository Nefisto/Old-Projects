using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyGroupSettings : MonoBehaviour, IEnumerable<EnemyBattleActor>
{
    [TitleGroup("Settings")]
    [SerializeField]
    private List<PositionSettings> positionSettings;

    [TitleGroup("Settings")]
    [SerializeField]
    private EnemyBattleActor prefab;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private List<EnemyBattleActor> enemies;

    public IEnumerator<EnemyBattleActor> GetEnumerator() => enemies.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CreateEnemiesInstance()
    {
        enemies.Clear();
        foreach (var settings in positionSettings)
        {
            var instance = Instantiate(
                original: prefab,
                position: settings.transform.position,
                rotation: Quaternion.identity,
                parent: transform);

            instance.transform.localScale = settings.transform.localScale;

            var battlePositionManager = instance.GetComponent<BattlePositionManager>();
            battlePositionManager.SetupBattleLinePosition(settings.battleLine);

            enemies.Add(instance);
        }
    }

    public void DestroyEnemies()
    {
        foreach (var child in enemies.Where(e => e != null).ToList())
            Destroy(child.gameObject);
        enemies.Clear();
    }

    public void HidePlaceholders()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void ShowPlaceholder()
    {
        foreach (var settings in positionSettings)
        {
            var battlePositionManager = settings.transform.GetComponent<BattlePositionManager>();
            battlePositionManager.SetupBattleLinePosition(settings.battleLine);

            settings.transform.gameObject.SetActive(true);
        }
    }

    [Serializable]
    public class PositionSettings
    {
        [HideLabel]
        [HorizontalGroup]
        public Transform transform;

        [HideLabel]
        [HorizontalGroup]
        public BattleLinePosition battleLine;
    }
}