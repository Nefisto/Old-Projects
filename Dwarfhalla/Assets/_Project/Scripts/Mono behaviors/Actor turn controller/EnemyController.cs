using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyController : ActorTurnController
{
    [TitleGroup("References")]
    [SerializeField]
    private EnemyAI enemyAI;

    [HideInEditorMode]
    [InlineEditor]
    public EnemyAI instance;

    private EnemyData enemyData;

    private void Awake()
    {
        instance = enemyAI.GetInstance;

        GameEntryPoints.OnFinishedSetup += _ =>
        {
            enemyData = ServiceLocator.GameContext.EnemyData;
            enemyData.CreateNewHand();
        };
    }

    public override IEnumerator TurnHandle()
    {
        yield return TriggerTurnStartOfPieces(UnitSide.Goblin);

        yield return instance.TurnHandle();

        yield return new WaitForSeconds(1f);

        CommonOperations.CancelNotificationOnAllGrid();
        yield return TriggerTurnEndOfPieces(UnitSide.Goblin);

        enemyData.DiscardHand();
        enemyData.CreateNewHand();
    }
}