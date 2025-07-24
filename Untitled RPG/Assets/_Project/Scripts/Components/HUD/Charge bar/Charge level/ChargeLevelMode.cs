using System.Collections;
using System.Collections.Generic;
using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ChargeLevelMode : ChargeMode
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Color32 completedColorForSingleMessage;

    [TitleGroup("Settings")]
    [SerializeField]
    private Color32 notCompletedColorForSingleMessage;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text singleMessage;

    [TitleGroup("References")]
    [SerializeField]
    private ChargeLevelBar levelPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private Transform levelBarFolder;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private List<ChargeLevelBar> levelBars = new();

    public override IEnumerator Setup (ChargeMode.Settings settings)
    {
        Clean();

        var correctSettings = settings as Settings;

        correctSettings.skill.OnUpdateHoldPoints += () =>
        {
            var (index, percentage) = correctSettings.skill.CurrentIndexAndPercentage;

            for (var i = index - 1; i >= 0; i--)
                levelBars[i].Fill(1f);

            levelBars[index].Fill(percentage);

            singleMessage.color = percentage.IsNearlyEnoughTo(1f)
                ? completedColorForSingleMessage
                : notCompletedColorForSingleMessage;

            singleMessage.fontStyle = percentage.IsNearlyEnoughTo(1f)
                ? FontStyles.Bold
                : FontStyles.Normal;
        };

        singleMessage.text = correctSettings.singleMessage;

        levelBars.Clear();
        foreach (var chargeLevelSettings2 in correctSettings.skill.ChargePointsSettings)
        {
            var instance = Instantiate(levelPrefab, levelBarFolder, false);
            levelBars.Add(instance);

            yield return instance.Setup(chargeLevelSettings2);
        }
    }

    private void Clean()
    {
        foreach (Transform child in levelBarFolder.transform)
            Destroy(child.gameObject);
    }

    public new class Settings : ChargeMode.Settings
    {
        public string singleMessage;
        public LevelChargeSkill skill;
    }
}