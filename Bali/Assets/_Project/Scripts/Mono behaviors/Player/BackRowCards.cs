using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackRowCards : MonoBehaviour, IEnumerable<BackRowCardSlot>
{
    [Title("References")]
    [SerializeField]
    private List<BackRowCardSlot> cardSlots;
    
    public int NumberOfAliveCards => cardSlots.Count(slot => slot.IsAlive);

    public IEnumerator<BackRowCardSlot> GetEnumerator()
        => cardSlots.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    public IEnumerator Setup (params CardData[] cardsToShow)
    {
        for (var i = 0; i < cardsToShow.Length; i++)
            cardSlots[i].Setup(cardsToShow[i]);

        yield return ShowNonEmptySlots();
    }

    private IEnumerator ShowNonEmptySlots()
    {
        foreach (var cardSlot in cardSlots)
        {
            if (cardSlot.IsEmptySlot())
                continue;

            cardSlot.ShowCardHUD();
        }

        yield break;
    }

    public IEnumerator SelectLeader (Action<OnChoseLeaderContext> afterChoseALeaderCallBack)
    {
        var clickTrigger = GetBackRowEventTriggers();

        var waitForUI = new WaitForUIEventTrigger(clickTrigger);
        yield return waitForUI;

        var context = new OnChoseLeaderContext
        {
            SelectedSlot = waitForUI.PressedButton.GetComponent<BackRowCardSlot>()
        };
        afterChoseALeaderCallBack?.Invoke(context);
    }

    public IEnumerable<EventTrigger> GetBackRowEventTriggers()
    {
        var clickTrigger = cardSlots
            .Select(cs => cs.EventTrigger);
        return clickTrigger;
    }

    public IEnumerator HideSlots()
    {
        foreach (var cardSlot in cardSlots)
            cardSlot.HideCardHUD();

        yield break;
    }
}

public class OnChoseLeaderContext
{
    public CardSlot SelectedSlot;
}