using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ExperienceCounterHUD : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private int framesToComplete = 120;

    [TitleGroup("References")]
    [SerializeField]
    private LevelUpIconAnimation levelUpIconAnimation;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text baseExperienceGainedLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text multiplierLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text totalExperienceGainedLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text currentLevelLabel;

    [TitleGroup("References")]
    [SerializeField]
    private Image experienceBar;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text toNextLevel;

    [TitleGroup("Debug")]
    [SerializeField]
    public bool shouldFinishNextFrame;

    public IEnumerator Run (BattleResultData resultData, float delayBeforeStart = 0f)
    {
        var playerData = ServiceLocator.SessionManager.PlayableCharacterData;
        var experienceContext = new ExperienceContext
        {
            currentLevel = playerData.CurrentLevel,
            currentExperience = playerData.CurrentExperience,
            baseExperienceGained = resultData.BaseExperienceGained,
            totalExperienceGained = resultData.GetTotalExp(),
            totalMultiplier = resultData.TotalBattleMultiplier
        };

        shouldFinishNextFrame = false;
        Setup(experienceContext);
        yield return new WaitForSeconds(delayBeforeStart);
        yield return InternalCountExperienceRoutine(experienceContext);
    }

    private IEnumerator InternalCountExperienceRoutine (ExperienceContext ctx)
    {
        var expGainedPerFrame = (float)ctx.totalExperienceGained / framesToComplete;
        var accumulateExp = 0f; // As the exp per frame can be a fraction, we need to accumulate it in some place

        while (ctx.totalExperienceGained > 0)
        {
            if (shouldFinishNextFrame)
            {
                PrematureFinishCounter(ctx);
                yield break;
            }

            yield return null;

            accumulateExp += Mathf.Min(expGainedPerFrame, ctx.totalExperienceGained);

            if (accumulateExp < 1)
                continue;

            ctx.totalExperienceGained -= (int)accumulateExp;
            ctx.currentExperience += (int)accumulateExp;
            ctx.couldLevelUp = ctx.currentExperience
                               >= Database.GameConstantsSo.ExperienceTable.TotalExperienceRequiredForLevel(
                                   ctx.currentLevel + 1);

            Setup(ctx);

            accumulateExp -= (int)accumulateExp;
        }
    }

    private void PrematureFinishCounter (ExperienceContext context)
    {
        var totalExp = context.currentExperience + context.totalExperienceGained;
        var level = Database.GameConstantsSo.ExperienceTable.GetLevelFromExperience(totalExp);

        Setup(new ExperienceContext
        {
            currentLevel = level,
            currentExperience = totalExp,
            totalExperienceGained = 0
        });
    }

    private void Setup (ExperienceContext context)
    {
        baseExperienceGainedLabel.text = $"{context.baseExperienceGained}";
        multiplierLabel.text = $"{context.totalMultiplier:F1}";
        totalExperienceGainedLabel.text = $"{context.totalExperienceGained}";
        currentLevelLabel.text = $"Level {context.currentLevel:00}";
        experienceBar.fillAmount = context.PercentageToNextLevel;
        toNextLevel.text = $"{context.ExperienceToNextLevel}";

        if (!context.couldLevelUp)
            return;

        context.currentLevel++;

        levelUpIconAnimation.PlayTween();

        context.couldLevelUp = false;
    }

    public class ExperienceContext
    {
        public int baseExperienceGained;
        public bool couldLevelUp;
        public int currentExperience;
        public int currentLevel;
        public int totalExperienceGained;
        public float totalMultiplier;

        public int ExperienceToNextLevel
            => currentLevel + 1 == GameConstants.LEVEL_CAP
                ? 99999999
                : Database.GameConstantsSo.ExperienceTable.TotalExperienceRequiredForLevel(currentLevel + 1)
                  - currentExperience;

        public float PercentageToNextLevel
        {
            get
            {
                var experienceTable = Database.GameConstantsSo.ExperienceTable;

                var lastLevelExpRequired = experienceTable.TotalExperienceRequiredForLevel(currentLevel);
                var nextLevelExpRequired = experienceTable.TotalExperienceRequiredForLevel(currentLevel + 1);

                return Mathf.Min(
                    ((float)currentExperience - lastLevelExpRequired) / (nextLevelExpRequired - lastLevelExpRequired),
                    1f);
            }
        }
    }
}