using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class EnemyData
{
    [TabGroup("Data tab", "Messages")]
    [SerializeField]
    private List<Dialog> possibleMessages = new List<Dialog>();

    public IEnumerable<Dialog> GetDialogs (DialogCommonEvents battleEvent, string customEvent = null)
        => battleEvent != DialogCommonEvents.Custom
            ? GetMessagesFromEvent(battleEvent)
            : GetMessagesFromCustomEvent(customEvent);

    private IEnumerable<Dialog> GetMessagesFromEvent (DialogCommonEvents battleEvent)
        => possibleMessages
            .Where(d => d.BattleEvent == battleEvent);

    private IEnumerable<Dialog> GetMessagesFromCustomEvent (string customEvent)
        => GetMessagesFromEvent(DialogCommonEvents.Custom)
            .Where(d => d.CustomEvent == customEvent);
}