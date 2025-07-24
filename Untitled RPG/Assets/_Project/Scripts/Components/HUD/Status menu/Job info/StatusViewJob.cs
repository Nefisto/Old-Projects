using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusViewJob : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image jobIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text jobName;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonus bonusInfo;

    public IEnumerator Setup (PlayableCharacterData characterData)
    {
        jobIcon.sprite = characterData.Job.JobIcon;
        jobName.text = characterData.Job.Name;

        yield return bonusInfo.Setup(characterData);
        SetupClickInteraction(characterData.Job);
    }

    private void SetupClickInteraction (GameJob job)
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => StartCoroutine(GameEvents.OpenGameJobViewEntryPoint.YieldableInvoke(job)));

        eventTrigger.triggers.Clear();
        eventTrigger.triggers.Add(entry);
    }
}