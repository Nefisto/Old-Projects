using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private CardViewCost costController;

    [TitleGroup("References")]
    [SerializeField]
    private Image iconImage;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private Transform cardFolder;

    [TitleGroup("Debug")]
    [ReadOnly]
    private bool canDrag;

    private NTask hoverTask;

    public Action OnDragBegin;
    public Action OnPointerExit;

    public ICard CardData { get; private set; }

    public void Setup (ICard card)
    {
        CardData = card;

        costController.SetPoints(card.Cost);
        iconImage.sprite = card.Icon;

        canDrag = true;

        SetupMouseInteractions();
    }

    public void HideCard() => cardFolder.gameObject.SetActive(false);

    public IEnumerator DisableDrag()
    {
        iconImage.color = iconImage.color.SetAlpha(0.2f);
        costController.SetAPIconsAlpha(0.2f);
        canDrag = false;
        yield break;
    }

    public IEnumerator EnabledDrag()
    {
        iconImage.color = iconImage.color.SetAlpha(1f);
        costController.SetAPIconsAlpha(1f);
        canDrag = true;
        yield break;
    }

    private void SetupMouseInteractions()
    {
        var beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.PointerDown;
        beginDragEntry.callback.AddListener(data =>
        {
            if (!canDrag)
                return;

            if (((PointerEventData)data).button != PointerEventData.InputButton.Left)
                return;

            OnDragBegin?.Invoke();
        });

        var pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener(_ =>
        {
            OnPointerExit?.Invoke();

            hoverTask?.Stop();
            ServiceLocator.Tooltip.HideTooltip();
        });

        var hoverEntry = new EventTrigger.Entry();
        hoverEntry.eventID = EventTriggerType.PointerEnter;
        hoverEntry.callback.AddListener(_ =>
        {
            if (CardData is not SummonCard)
                return;

            hoverTask = new NTask(HoverTimer());
        });

        eventTrigger.triggers.Add(hoverEntry);
        eventTrigger.triggers.Add(beginDragEntry);
        eventTrigger.triggers.Add(pointerExitEntry);
    }

    private IEnumerator HoverTimer()
    {
        yield return new WaitForSeconds(0.75f);

        ServiceLocator.Tooltip.Setup(CardData);
        ServiceLocator.Tooltip.ShowTooltip();
    }
}