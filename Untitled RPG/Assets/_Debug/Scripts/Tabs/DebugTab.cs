using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugTab : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Transform syncedContent;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    public event Action onPressed;

    public void Setup()
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => onPressed?.Invoke());

        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(entry);

        onPressed = null;
    }

    public void Open() => syncedContent.gameObject.SetActive(true);
    public void Close() => syncedContent.gameObject.SetActive(false);
}