using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotEntrySetupContext
{
    public SlotClickInteractions clickInteractions;
    public EquipmentData equipmentData;
    public Action OnUpdateSlot;
}

[RequireComponent(typeof(EventTrigger))]
public abstract class SlotEntry : MonoBehaviour, IEquipmentSlot
{
    [TitleGroup("References")]
    [SerializeField]
    private Image icon;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    private Func<BaseEventData, IEnumerator> cachedHoldingRoutine;
    private Func<BaseEventData, IEnumerator> holdingRoutine;

    private void OnDestroy() => StopAllCoroutines();

    public virtual IEnumerator Setup (SlotEntrySetupContext context)
    {
        RefreshView(context.equipmentData);
        ClearBehaviors();

        yield break;
    }

    protected virtual void ClearBehaviors() => eventTrigger.triggers.Clear();

    protected virtual void AddHoldingBehavior (Func<BaseEventData, IEnumerator> successCallback)
    {
        cachedHoldingRoutine = successCallback;

        var holdBegin = new EventTrigger.Entry();
        holdBegin.eventID = EventTriggerType.PointerDown;
        holdBegin.callback.AddListener(data =>
        {
            holdingRoutine = cachedHoldingRoutine;
            StartCoroutine(holdingRoutine.Invoke(data));
        });

        var holdEnd = new EventTrigger.Entry();
        holdEnd.eventID = EventTriggerType.PointerUp;
        holdEnd.callback.AddListener(data => { StopAllCoroutines(); });

        eventTrigger.triggers.Add(holdBegin);
        eventTrigger.triggers.Add(holdEnd);
    }

    protected void AddClickBehavior (Action<BaseEventData> successCallback)
        => AddEvent(successCallback, EventTriggerType.PointerClick);

    public virtual void RefreshView (EquipmentData equipmentData) => icon.sprite = equipmentData.Icon;

    private void AddEvent (Action<BaseEventData> OnSuccess, EventTriggerType eventID)
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(data => OnSuccess?.Invoke(data));

        eventTrigger.triggers.Add(entry);
    }
}