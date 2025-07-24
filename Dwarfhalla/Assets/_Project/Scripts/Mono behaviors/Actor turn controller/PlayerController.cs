using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : ActorTurnController
{
    [TitleGroup("References")]
    [SerializeField]
    private MouseController mouseController;

    [TitleGroup("References")]
    [SerializeField]
    private Button endTurnButton;

    private ActionPointsView actionPointsView;

    private HandView handView;
    private PlayerData playerData;

    private bool shouldFinishTurnEarly;

    private void Awake()
    {
        GameEntryPoints.OnFinishedSetup += _ =>
        {
            handView = ServiceLocator.HandView;
            actionPointsView = ServiceLocator.ActionPointsView;
            playerData = ServiceLocator.GameContext.PlayerData;

            playerData.ActionPoints.CurrentPoints = 0;
        };

        endTurnButton.onClick.RemoveAllListeners();
        endTurnButton.onClick.AddListener(() => shouldFinishTurnEarly = true);
    }

    public override IEnumerator TurnHandle()
    {
        var turnContext = ServiceLocator.GameContext.TurnContext;

        playerData.ActionPoints.Recharge();
        yield return actionPointsView.RefreshVisual();

        shouldFinishTurnEarly = false;
        yield return TriggerTurnStartOfPieces(UnitSide.Dwarf);
        while (playerData.ActionPoints.CurrentPoints > 0)
        {
            turnContext.Reset();

            yield return handView.EnableCardsDrag();
            yield return WaitForCardToBeSelected();

            if (shouldFinishTurnEarly)
                break;

            yield return WaitForInitialBlockToBeSelected(turnContext);

            ServiceLocator.MouseController.RemoveOnHoveringOnNoBlocksOperation();
            RemoveBlocksHoverOperation();
            if (!HasSelectedValidInitialBlock(turnContext))
            {
                CommonOperations.CancelNotificationOnAllGrid();
                continue;
            }

            yield return turnContext.SelectedCard.Perform();
            if (!turnContext.HasSuccessfullyPerformedAction)
                continue;

            playerData.ActionPoints.CurrentPoints -= turnContext.SelectedCard.Cost;
            yield return GameEntryPoints.OnPlayerPerformedAction?.YieldableInvoke();
            playerData.Hand.RemoveCard(turnContext.SelectedCard);
            yield return actionPointsView.RefreshVisual();
            yield return CommonOperations.ProcessDeathAnimation();
        }

        playerData.DiscardHand();
        playerData.CreateNewHand();
        yield return handView.CreateHand();

        yield return TriggerTurnEndOfPieces(UnitSide.Dwarf);
    }

    private static bool HasSelectedValidInitialBlock (TurnContext turnContext)
        => turnContext.TargetBlock is not null
           && turnContext.SelectedCard.CanBePerformed();

    private IEnumerator WaitForInitialBlockToBeSelected (TurnContext turnContext)
    {
        ServiceLocator.MouseController.SetupOnHoveringOnTopOfNothing(() =>
        {
            ServiceLocator.GameContext.TurnContext.TargetBlock = null;
            CommonOperations.CancelNotificationOnAllGrid();
        });
        SetBlocksToPreviewSelectedCard();
        SetMouseButtonUpToDetectInitialBlock();
        yield return new WaitUntil(() => turnContext.HasSelectedInitialBlock);
        CommonOperations.GetAllBlocksOnCurrentRoom().ForEach(b => b.RuntimeBlock.RemoveHoverOperation());
        ServiceLocator.MouseController.RemoveMouseButtonUpOperation();
    }

    private IEnumerator WaitForCardToBeSelected()
    {
        yield return new WaitUntil(()
            => ServiceLocator.GameContext.TurnContext.SelectedCard is not null || shouldFinishTurnEarly);
    }

    private static void RemoveBlocksHoverOperation()
    {
        CommonOperations
            .GetAllBlocksOnCurrentRoom()
            .ForEach(block => block.RuntimeBlock.RemoveHoverOperation());
    }

    private static void RemoveBlockClickOperation()
    {
        CommonOperations
            .GetAllRuntimeBlocksOnCurrentRoom()
            .ForEach(block => block.RemoveClickOperation());
    }

    private void SetMouseButtonUpToDetectInitialBlock()
    {
        mouseController.SetMouseButtonUpOperation(() =>
        {
            var turnContext = ServiceLocator.GameContext.TurnContext;
            handView.DisableDragIcon();

            turnContext.HasSelectedInitialBlock = true;
        });
    }

    private static void SetBlocksToPreviewSelectedCard()
    {
        var turnContext = ServiceLocator.GameContext.TurnContext;
        var allBlocks = CommonOperations.GetAllBlocksOnCurrentRoom();

        allBlocks.ForEach(block => block.RuntimeBlock.SetHoverOperation(() =>
        {
            if (turnContext == null)
                return;

            turnContext.TargetBlock = block;

            CommonOperations.CancelNotificationOnAllGrid();

            if (!turnContext.SelectedCard.CanBePerformed())
            {
                turnContext.TargetBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.ImpossibleBlock);
                return;
            }

            turnContext.SelectedCard?.PreviewExecution();
        }));
    }
}