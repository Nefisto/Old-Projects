using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class JobSkillDetail : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image featIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text featName;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text featDescription;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    public IEnumerator Setup (Skill skill)
    {
        featIcon.sprite = skill.Icon;
        featName.text = skill.Name;
        featDescription.text = skill.Description;
        SetupButtonInteraction(skill);
        yield break;
    }

    private void SetupButtonInteraction (Skill skill)
    {
        eventTrigger.triggers.Clear();

        if (skill is NullSpecialSkill)
            return;

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ =>
        {
            StartCoroutine(
                GameEvents.OpenDetailedSkillInfoEntryPoint.YieldableInvoke(
                    new SkillDetailedInfo.Settings() { skill = skill }));
        });

        eventTrigger.triggers.Add(entry);
    }
}