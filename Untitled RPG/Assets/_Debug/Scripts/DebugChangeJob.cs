using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugChangeJob : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Dropdown dropdown;

    [TitleGroup("References")]
    [SerializeField]
    private Button applyButton;

    private void Awake()
    {
        GameEvents.onFinishedLoadingData += UpdateDropDown;
        applyButton.onClick.RemoveAllListeners();
        applyButton.onClick.AddListener(() =>
        {
            var jobName = dropdown.options[dropdown.value].text;
            var selectedJob = Database.GameJobs.Data.First(j => j.Name == jobName);

            ServiceLocator.SessionManager.PlayableCharacterData.ChangeJob(selectedJob);
        });
    }

    private void UpdateDropDown()
    {
        dropdown.options.Clear();
        foreach (var gameJob in Database.GameJobs.Data)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = gameJob.Name });
        }

        dropdown.RefreshShownValue();
    }
}