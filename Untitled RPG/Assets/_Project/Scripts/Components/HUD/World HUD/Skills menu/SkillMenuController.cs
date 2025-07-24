using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
[SelectionBase]
public class SkillMenuController : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private SkillMenu skillMenu;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    private void Awake()
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(_ => StartCoroutine(Open()));

        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(entry);
    }

    private void Reset() => eventTrigger ??= GetComponent<EventTrigger>();

    private IEnumerator Open()
    {
        ServiceLocator.MenuStack.OpenMenu(skillMenu);
        yield break;
    }
}