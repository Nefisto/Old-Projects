using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClickBehavior : ScriptableObject
{
    public virtual void AddClickBehavior (ClickBehaviorContext ctx)
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => Behavior(ctx));
        
        ctx.eventTrigger.triggers.Add(entry);
    }

    protected abstract void Behavior(ClickBehaviorContext ctx);
}

public class ClickBehaviorContext
{
    public EventTrigger eventTrigger;
    public EquipmentData equipmentData;
    
    public Action onCloseMenu;
}