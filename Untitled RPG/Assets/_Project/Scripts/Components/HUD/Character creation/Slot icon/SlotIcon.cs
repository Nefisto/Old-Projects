using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotIcon : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private SlotIconGrid slotIconGrid;

    public IEnumerator Setup()
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => ServiceLocator.MenuStack.OpenMenu(slotIconGrid));
        eventTrigger.triggers.Add(entry);
        yield break;
    }
}