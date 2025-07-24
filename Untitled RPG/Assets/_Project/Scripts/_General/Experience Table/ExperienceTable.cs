using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public partial class ExperienceTable
{
    [TabGroup("Experience", "To next level")]
    [DetailedInfoBox("Experience to each level", "@ToNextLevelInfoBox()", InfoMessageType.None)]
    [OnValueChanged(nameof(RefreshTable))]
    [SerializeField]
    private AnimationCurve toNextLevel;

    [field: TabGroup("Experience", "Total")]
    [field: DetailedInfoBox("Experience to each level", "@TotalInfoBox()", InfoMessageType.None)]
    [field: SerializeField]
    public AnimationCurve Table { get; private set; }

    public int ExperienceRequiredForLevel (int level) => (int)toNextLevel.Evaluate(level);
    public int TotalExperienceRequiredForLevel (int level) => (int)Table.Evaluate(level);

    public int GetLevelFromExperience (int experience)
    {
        var level = 1;
        for (var i = 1; i < GameConstants.LEVEL_CAP; i++)
        {
            var required = ExperienceRequiredForLevel(i);

            if (required <= experience)
            {
                experience -= required;
                level++;
            }
            else
            {
                break;
            }
        }

        return level;
    }
}