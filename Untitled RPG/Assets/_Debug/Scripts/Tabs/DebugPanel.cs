using System.Collections.Generic;
using NTools;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    public DebugTab worldTab;
    public DebugTab battleTab;

    private void Start()
    {
        foreach (var debugTab in TabsIterator())
        {
            debugTab.Setup();
            debugTab.onPressed += () => OpenTab(debugTab);
        }

        OpenTab(worldTab);

        GameEvents.OnBattleFinishedEntryPoint += _ => OpenTab(worldTab);
        GameEvents.onBattleTriggered += _ => OpenTab(battleTab);
    }

    private void OpenTab (DebugTab tab)
    {
        TabsIterator().ForEach(t => t.Close());
        tab.Open();
    }

    private IEnumerable<DebugTab> TabsIterator()
    {
        yield return worldTab;
        yield return battleTab;
    }
}