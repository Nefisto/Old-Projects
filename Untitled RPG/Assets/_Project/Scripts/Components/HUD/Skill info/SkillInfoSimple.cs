using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public partial class SkillInfoSimple : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private Image skillIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text skillName;

    [TitleGroup("References")]
    [SerializeField]
    private List<AttributeInfoEntry2> attributeInfoEntries;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Skill setSkill;

    public IEnumerator Setup (Skill skill)
    {
        setSkill = skill;
        if (skill is null or DoNothingSkill)
        {
            yield return SetupEmpty();
            yield break;
        }

        skillIcon.enabled = true;
        skillIcon.sprite = skill.Icon;
        skillName.text = skill.name;

        var hudEntryToAttributeInfo = Enumerable
            .Zip(attributeInfoEntries, skill.MainAttributesIterator(),
                (entry, info) => (entry, info));

        foreach (var (entry, info) in hudEntryToAttributeInfo)
            yield return entry.Setup(info);

        SetupClickInteraction();
    }

    private void SetupClickInteraction()
    {
        eventTrigger.triggers.Clear();

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(OpenDetailedSkillView);

        eventTrigger.triggers.Add(entry);
    }

    private void OpenDetailedSkillView (BaseEventData _)
    {
        StartCoroutine(Behavior());

        IEnumerator Behavior()
        {
            yield return GameEvents.OpenDetailedSkillInfoEntryPoint.YieldableInvoke(new SkillDetailedInfo.Settings()
            {
                skill = setSkill
            });
        }
    }

    private IEnumerator SetupEmpty()
    {
        skillIcon.enabled = false;

        skillIcon.sprite = null;
        skillName.text = "---";

        foreach (var attributeInfoEntry in attributeInfoEntries)
            yield return attributeInfoEntry.Setup(new Skill.AttributeOnHUD("-", "-"));
    }
}