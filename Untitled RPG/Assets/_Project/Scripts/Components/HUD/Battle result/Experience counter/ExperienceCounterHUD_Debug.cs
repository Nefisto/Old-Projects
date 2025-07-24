using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
public partial class ExperienceCounterHUD
{
    [Title("Debug")]
    [DisableInEditorButton]
    private void RunCounter (int baseExperienceGained = 100, float totalMultiplier = 1f, int playerLevel = 1, int playerCurrentExperience = 0)
    {
        StartCoroutine(InternalCountExperienceRoutine(new ExperienceContext
        {
            currentExperience = playerCurrentExperience,
            currentLevel = playerLevel,
            baseExperienceGained = baseExperienceGained,
            totalMultiplier = totalMultiplier,
            totalExperienceGained = Mathf.RoundToInt(baseExperienceGained * totalMultiplier),
        }));
    }
}
#endif