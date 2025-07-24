using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class CombatLog : MonoBehaviour
{
    [TabGroup("Basic")]
    [Title("Control")]
    [SerializeField]
    private RectTransform contentFolder;

    [Space]
    [TabGroup("Basic")]
    [SerializeField]
    private GameObject entryPrefab;

    private Stack<CombatLogEntry> logHistory = new Stack<CombatLogEntry>();

    private void Awake()
        => SetupCollapse();

    private void OnEnable()
    {
        GameEvents.Battle.OnSetupBattle += SetupBattleListener;
        GameEvents.Battle.OnLogBattleAction += Log;
    }

    private void OnDisable()
    {
        GameEvents.Battle.OnSetupBattle -= SetupBattleListener;
        GameEvents.Battle.OnLogBattleAction -= Log;
    }

    public void SetupBattleListener (BattleEncounterContext _)
        => ClearCombatLog();

    public void ClearCombatLog()
    {
        for (var i = contentFolder.childCount - 1; i >= 0; i--)
            Destroy(contentFolder.GetChild(i).gameObject);
    }

    [Button]
    private void ShowLog()
    {
        foreach (var combatLogEntry in logHistory)
        {
            Debug.Log($"{combatLogEntry}");
        }
    }

    private void Log (CombatLogEntry entry)
    {
        var instance = Instantiate(entryPrefab, contentFolder, false)
            .GetComponent<LogEntry>();
        
        instance.Setup(entry);
        
        logHistory.Push(entry);
    }
}