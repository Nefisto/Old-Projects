using Loot;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Slippy goblin brain", menuName = EditorConstants.MenuAssets.ENEMY_BRAIN + "Slippy goblin",
    order = 0)]
public class SlippyGoblinBrain : EnemyBrain
{
    [TitleGroup("Settings", order: -15)]
    [SerializeField]
    private Steal steal;

    [TitleGroup("Settings")]
    [SerializeField]
    private EnemyCoinThrow coinThrow;

    [TitleGroup("Settings")]
    [SerializeField]
    private int minimumCoinsToCastThrow = 20;

    [TitleGroup("Settings")]
    [SerializeField]
    private int minimumTurnsBetweenSteal = 3;

    [TitleGroup("Settings")]
    [SerializeField]
    private int turnsBetweenCoinThrow = 2;

    private BattleActor battleActor;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int turnsFromLastCoinThrow;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private int turnsFromLastSteal;

    public override void ActUpdate()
    {
        turnsFromLastSteal++;
        turnsFromLastCoinThrow++;

        if (CanReenableSteal())
            skillNameToRule[steal.Name].IsDisabled = false;

        if (CanReenableCoinThrow())
            skillNameToRule[coinThrow.Name].IsDisabled = false;
    }

    public override Drop[] GetBehaviorRules (BehaviorContext context)
    {
        battleActor = context.battleActor;
        var stealInstance = (Steal)steal.GetInstance;
        var stealDrop = new Drop()
        {
            Entry = stealInstance,
            Weight = GameConstants.SKILL_WEIGHT_MID
        };

        stealInstance.OnCast += () => turnsFromLastSteal = 0;
        stealInstance.OnSuccessfullyStole += _ => stealDrop.IsDisabled = true;

        var coinThrowInstance = (EnemyCoinThrow)coinThrow.GetInstance;
        var coinThrowDrop = new Drop()
        {
            Entry = coinThrowInstance,
            Weight = GameConstants.SKILL_WEIGHT_MID,
            IsDisabled = true
        };
        coinThrowInstance.OnCast += () =>
        {
            if (battleActor.Currency < minimumCoinsToCastThrow)
                coinThrowDrop.IsDisabled = true;
            turnsFromLastCoinThrow = 0;
        };

        var attackDrop = new Drop
        {
            Entry = context.enemyData.DefaultAttackSkill,
            Weight = GameConstants.SKILL_WEIGHT_LOW
        };

        skillNameToRule.Add(steal.Name, stealDrop);
        skillNameToRule.Add(coinThrow.Name, coinThrowDrop);

        return new[] { attackDrop, stealDrop, coinThrowDrop };
    }

    private bool CanReenableCoinThrow()
        => battleActor.Currency >= minimumCoinsToCastThrow && turnsFromLastCoinThrow >= turnsBetweenCoinThrow;

    private bool CanReenableSteal()
        => battleActor.Currency < minimumCoinsToCastThrow && turnsFromLastSteal >= minimumTurnsBetweenSteal;
}