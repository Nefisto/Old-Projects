using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle result", menuName = EditorConstants.MenuAssets.SERVICES + "Battle result",
    order = 0)]
public partial class BattleResultData : ScriptableObject, IList<OnDieContext>
{
    public enum BattleWinner
    {
        None,
        Player,
        Enemy
    }

    [field: TitleGroup("Debug")]
    [field: SerializeField]
    public BattleWinner Winner { get; set; }

    [field: TitleGroup("Debug")]
    [field: SerializeField]
    public float BaseExperienceMultiplier { get; private set; } = 1f;

    [field: TitleGroup("Debug")]
    [field: SerializeField]
    public List<ExperienceMultiplier> ExperienceMultipliers { get; private set; } = new();

    [field: TitleGroup("Debug")]
    [field: SerializeField]
    public List<OnDieContext> BattleResult { get; private set; } = new();

    [TitleGroup("Debug")]
    [ShowInInspector]
    public float TotalBattleMultiplier
        => BaseExperienceMultiplier + ExperienceMultipliers.Sum(m => m.MultiplierIncrease);

    [TitleGroup("Debug")]
    [ShowInInspector]
    public int BaseExperienceGained => BattleResult.Sum(ctx => ctx.experienceReceived);

    public void AddMultiplier (ExperienceMultiplier multiplier) => ExperienceMultipliers.Add(multiplier);

    public int GetTotalExp() => Mathf.RoundToInt(BaseExperienceGained * TotalBattleMultiplier);

    public int GetTotalCurrency() => BattleResult.Sum(ctx => ctx.currency);
}