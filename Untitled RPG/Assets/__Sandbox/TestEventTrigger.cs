using UnityEngine;
using UnityEngine.EventSystems;

public class TestEventTrigger : MonoBehaviour
{
    public EventTrigger trigger;

    private void Awake()
    {
        trigger = GetComponent<EventTrigger>();

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => Debug.Log("Hello"));

        trigger.triggers.Add(entry);
    }
}