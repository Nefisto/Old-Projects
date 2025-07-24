using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoCardController : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private RectTransform clickableArea;
    
    [SerializeField]
    private InfoCard infoCard;
    
    private void Awake()
    {
        GameEvents.Battle.OnSetupBattle += SetupBattleListener;
    }

    private void Start()
    {
        AddTriggerToClickableArea();
    }

    private void AddTriggerToClickableArea()
    {
        var clickableTrigger = clickableArea.GetComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => { HideInfoCard(); });
        
        clickableTrigger.triggers.Add(entry);
    }

    public void ShowInfoCard(InfoCardContext ctx)
    {
        clickableArea.gameObject.SetActive(true);
        infoCard.gameObject.SetActive(true);
        infoCard.ShowInfoCard(ctx);
    }

    public void HideInfoCard()
    {
        clickableArea.gameObject.SetActive(false);
        infoCard.gameObject.SetActive(false);
    }

    private void SetupBattleListener (BattleEncounterContext _)
    {
        if (clickableArea != null)
            clickableArea.gameObject.SetActive(false);
        
        if (infoCard != null)
            infoCard.gameObject.SetActive(false);
    }
}