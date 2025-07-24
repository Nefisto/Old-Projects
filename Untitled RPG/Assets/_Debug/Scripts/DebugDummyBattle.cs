using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DebugDummyBattle : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private EnemyDataFactory dummyEnemy;

    [TitleGroup("References")]
    [SerializeField]
    private Button triggerButton;

    private void Awake()
    {
        GameEvents.OnBeginningAdventure += _ => triggerButton.interactable = true;

        triggerButton.interactable = false;
        triggerButton.onClick.RemoveAllListeners();
        triggerButton.onClick.AddListener(() => GameEvents.onBattleTriggered?.Invoke(new BattleSetupContext()
        {
            enemiesData = new List<EnemyData>
            {
                dummyEnemy.GetInstance(),
                dummyEnemy.GetInstance(),
                dummyEnemy.GetInstance()
            }
        }));
    }
}