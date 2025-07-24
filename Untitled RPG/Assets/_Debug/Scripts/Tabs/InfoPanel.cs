using System.Collections.Generic;
using NTools;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public DebugTab soFarTab;
    public DebugTab tutorialTab;

    private void Start()
    {
        foreach (var debugTab in TabsIterator())
        {
            debugTab.Setup();
            debugTab.onPressed += () => OpenTab(debugTab);
        }

        OpenTab(soFarTab);

        GameEvents.OnBattleFinishedEntryPoint += _ => OpenTab(soFarTab);
        GameEvents.onBattleTriggered += _ => OpenTab(tutorialTab);
    }

    private void OpenTab (DebugTab tab)
    {
        TabsIterator().ForEach(t => t.Close());
        tab.Open();
    }

    private IEnumerable<DebugTab> TabsIterator()
    {
        yield return soFarTab;
        yield return tutorialTab;
    }
}