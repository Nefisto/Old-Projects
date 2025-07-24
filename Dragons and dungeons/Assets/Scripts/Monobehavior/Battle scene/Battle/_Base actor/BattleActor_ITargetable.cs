using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract partial class BattleActor
{
    [TabGroup("General")]
    [Title("Targetable")]
    [SerializeField]
    private GameObject targetSymbol;

    private EventTrigger.Entry setTargetEntry;

    public bool IsMarkedAsTarget => BattleManager.Instance.ContainAsTarget(this);

    public void EnableTarget ()
    {
        eventTrigger.triggers.Add(setTargetEntry);

        AppearAboveBlackBackground();
    }

    public void DisableTarget()
    {
        eventTrigger.triggers.Remove(setTargetEntry);

        AppearBellowBackground();
        HideTargetMark();
    }

    private void SetupTargetableEntry()
    {
        setTargetEntry = new EventTrigger.Entry();

        setTargetEntry.eventID = EventTriggerType.PointerClick;
        setTargetEntry.callback.AddListener(_ =>
        {
            if (IsMarkedAsTarget)
                UnMarkAsTarget();
            else
                MarkAsTarget();
        });
    }
    
    private void MarkAsTarget()
    {
        if (BattleManager.Instance.TryAddTarget(this))
            ShowTargetMark();
    }

    private void UnMarkAsTarget()
    { 
        HideTargetMark();

        BattleManager.Instance.RemoveTarget(this);
    }

    private void HideTargetMark()
        => targetSymbol.SetActive(false);

    private void ShowTargetMark()
        => targetSymbol.SetActive(true);

    private void AppearBellowBackground()
        => spriteRenderer.sortingOrder = 0;

    private void AppearAboveBlackBackground()
        => spriteRenderer.sortingOrder = 2;
}