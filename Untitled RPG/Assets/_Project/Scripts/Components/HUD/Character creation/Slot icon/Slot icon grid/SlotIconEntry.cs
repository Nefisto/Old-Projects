using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotIconClickContext
{
    public Sprite icon;
    public SaveSlotIconEnum iconEnum;
    public bool isUnlocked;
}

public class SlotIconEntry : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image background;

    [TitleGroup("References")]
    [SerializeField]
    private Image slotIcon;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private SaveSlotIconEnum iconEnum;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool isUnlocked;

    public event Action<SlotIconClickContext> OnClick;

    public IEnumerator Setup (SaveSlotIconEnum iconEnum, Sprite icon, bool isUnlocked)
    {
        OnClick = null;
        this.iconEnum = iconEnum;
        slotIcon.sprite = icon;
        SetupLock(isUnlocked);
        SetupClickInteraction();
        yield break;
    }

    private void SetupClickInteraction()
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(data =>
        {
            var ctx = new SlotIconClickContext();
            ctx.icon = slotIcon.sprite;
            ctx.isUnlocked = isUnlocked;
            ctx.iconEnum = iconEnum;

            OnClick?.Invoke(ctx);
        });

        eventTrigger.triggers.Add(entry);
    }

    private void SetupLock (bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
        background.color = isUnlocked ? Color.green : Color.gray;
        slotIcon.color = isUnlocked ? Color.white : Color.black;
    }
}