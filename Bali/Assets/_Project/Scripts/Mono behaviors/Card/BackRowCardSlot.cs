using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackRowCardSlot : CardSlot
{
    // This is responsible to bring the cards above others cards
    private Canvas cardCanvas;
    public EventTrigger EventTrigger { get; private set; }

    private void OnTriggerEnter2D (Collider2D col)
    {
        Debug.Log($"HI");
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        Debug.Log($"BYE");
    }

    private void Awake()
    {
        EventTrigger = GetComponent<EventTrigger>();
        cardCanvas = GetComponent<Canvas>();
    }

    public void OverlapCard()
        => cardCanvas.overrideSorting = true;

    public void UnderlayCard()
        => cardCanvas.overrideSorting = false;
}