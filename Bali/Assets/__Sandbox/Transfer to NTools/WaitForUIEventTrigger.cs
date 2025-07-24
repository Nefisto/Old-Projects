using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class WaitForUIEventTrigger : CustomYieldInstruction, IDisposable
{
    private readonly List<EventTriggerCallBack> buttons = new();
    private Action<EventTrigger> onClickedCallback;

    public WaitForUIEventTrigger (EventTriggerType triggerType = EventTriggerType.PointerClick,
        Action<EventTrigger> onClickedCallback = null,
        params EventTrigger[] buttons)
    {
        this.onClickedCallback = onClickedCallback;
        this.buttons.Capacity = buttons.Length;
        foreach (var b in buttons)
        {
            if (b == null)
                continue;

            var bc = CreateEventTriggerCallback(triggerType, b);

            this.buttons.Add(bc);
        }

        Reset();
    }

    public WaitForUIEventTrigger (params EventTrigger[] buttons) : this(EventTriggerType.PointerClick, null, buttons) { }

    public WaitForUIEventTrigger (IEnumerable<EventTrigger> buttons) : this(EventTriggerType.PointerClick, null, buttons.ToArray()) { }

    public override bool keepWaiting => PressedButton == null;
    public EventTrigger PressedButton { get; private set; }

    public void Dispose()
    {
        RemoveListeners();
        onClickedCallback = null;
        buttons.Clear();
    }

    private EventTriggerCallBack CreateEventTriggerCallback (EventTriggerType triggerType, EventTrigger b)
    {
        var bc = new EventTriggerCallBack { EventTrigger = b };

        bc.Entry = new EventTrigger.Entry();
        bc.Entry.callback.AddListener(x => OnButtonPressed(bc.EventTrigger));
        bc.Entry.eventID = triggerType;
        return bc;
    }

    private void OnButtonPressed (EventTrigger button)
    {
        PressedButton = button;
        RemoveListeners();

        onClickedCallback?.Invoke(button);
    }

    private void InstallListeners()
    {
        foreach (var bc in buttons.Where(bc => bc.EventTrigger != null))
            bc.EventTrigger.triggers.Add(bc.Entry);
    }

    private void RemoveListeners()
    {
        foreach (var bc in buttons.Where(bc => bc.EventTrigger != null))
            bc.EventTrigger.triggers.Remove(bc.Entry);
    }

    public new WaitForUIEventTrigger Reset()
    {
        RemoveListeners();
        PressedButton = null;
        InstallListeners();
        base.Reset();
        return this;
    }

    public WaitForUIEventTrigger ReplaceCallback (Action<EventTrigger> aCallback)
    {
        onClickedCallback = aCallback;
        return this;
    }

    private struct EventTriggerCallBack
    {
        public EventTrigger EventTrigger;
        public EventTrigger.Entry Entry;
    }
}