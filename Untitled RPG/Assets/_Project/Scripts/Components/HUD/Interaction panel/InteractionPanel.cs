using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class InteractionPanel : MonoBehaviour
{
    [TitleGroup("Debug")]
    [SerializeField]
    private EventTrigger eventTrigger;

    private void Awake() => eventTrigger = GetComponent<EventTrigger>();

    public void Setup (Action<BaseEventData> callback)
    {
        eventTrigger.triggers.Clear();

        AddCallback(callback);
    }

    private void AddCallback (Action<BaseEventData> callback)
    {
        var entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(data => callback?.Invoke(data));

        eventTrigger.triggers.Add(entry);
    }
}