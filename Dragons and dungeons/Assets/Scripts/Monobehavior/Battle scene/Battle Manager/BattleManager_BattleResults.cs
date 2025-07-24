using Sirenix.OdinInspector;
using UnityEngine;

public partial class BattleManager
{
    [TabGroup("Result")]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private BattleResult battleResult;

    public void AddPointsToBattleResult(BattleResult result)
    {
        battleResult.beautyPoints += result.beautyPoints;
        battleResult.madnessPoints += result.madnessPoints;
        battleResult.experiencePoints += result.experiencePoints;
    }

    private void ResetBattleResults()
        => battleResult = new BattleResult();
}