using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyDataInitializer : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private List<Card> initialCards;

    [TitleGroup("Debug")]
    [SerializeField]
    private EnemyData enemyData;

    private void Awake()
    {
        GameEntryPoints.GeneratingSessionData += DataHandle;
    }

    private IEnumerator DataHandle (object _)
    {
        enemyData = new EnemyData() { Deck = new Deck(initialCards
            .Select(c => c.GetInstance)
            .ToArray()) };

        ServiceLocator.GameContext.EnemyData = enemyData;

        yield return null;
    }
}