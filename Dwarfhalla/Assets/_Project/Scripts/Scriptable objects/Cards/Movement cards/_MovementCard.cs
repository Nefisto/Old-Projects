using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class MovementCard : Card
{
    [TitleGroup("Settings")]
    [Range(1, 9)]
    [SerializeField]
    protected int range = 2;

    protected bool hasFilledAdditionalInformation;

    protected List<BlockData> PossibleBlocks { get; set; } = new();

    protected abstract List<Vector2Int> GetPossiblePositions();

    public override bool CanBePerformed() => TurnContext.TargetBlock.UnitData?.CanMove ?? false;

    public override void PreviewExecution()
    {
        var startBlock = TurnContext.TargetBlock;
        var possiblePositions = GetPossiblePositions();

        CommonOperations.CancelNotificationOnAllGrid();
        PossibleBlocks.Clear();
        foreach (var possiblePosition in possiblePositions)
        {
            var targetBlock = CommonOperations.GetBlockDataAt(possiblePosition);

            if (targetBlock.HasUnitOnIt && targetBlock.UnitData.UnitSide == startBlock.UnitData.UnitSide)
            {
                targetBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.ImpossibleBlock);
                continue;
            }

            if (!CommonOperations.CheckForFreeViewBetween(startBlock.Position, possiblePosition))
            {
                targetBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.ImpossibleBlock);
                continue;
            }

            targetBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.PossibleBlock);
            PossibleBlocks.Add(targetBlock);
        }
    }

    protected IEnumerator FillAdditionalInformation()
    {
        var turnContext = ServiceLocator.GameContext.TurnContext;
        var cancelButton = ServiceLocator.CancelAction;
        var earlyCanceled = false;

        cancelButton.SetCancelBehavior(() => earlyCanceled = true);
        cancelButton.Show();
        AddHoverOperationOnPossibleBlocks(turnContext);
        AddClickOperationOnPossibleBlocks(turnContext);

        yield return ServiceLocator.BackgroundImage.ShowImage(new BackgroundImage.Settings { alphaToSet = 0.25f });
        hasFilledAdditionalInformation = false;
        yield return new WaitUntil(() => hasFilledAdditionalInformation || earlyCanceled);
        yield return ServiceLocator.BackgroundImage.HideImage();
        cancelButton.Hide();

        PossibleBlocks.ForEach(block =>
        {
            block.RuntimeBlock.RemoveHoverOperation();
            block.RuntimeBlock.RemoveClickOperation();
        });

        CommonOperations.CancelNotificationOnAllGrid();
    }

    public override IEnumerator Perform (object context)
    {
        yield return FillAdditionalInformation();

        if (!hasFilledAdditionalInformation)
            yield break;

        ServiceLocator.GlobalNotifyObject.SetActive(true);
        yield return new Movement()
        {
            InitialBlock = TurnContext.TargetBlock,
            FinalBlock = TurnContext.FinalBlock,
        }.Run();
        ServiceLocator.GlobalNotifyObject.SetActive(false);

        TurnContext.HasSuccessfullyPerformedAction = true;
    }

    protected void AddClickOperationOnPossibleBlocks (TurnContext turnContext)
    {
        PossibleBlocks.ForEach(block => block.RuntimeBlock.SetClickOperation(() =>
        {
            turnContext.FinalBlock = block;
            hasFilledAdditionalInformation = true;
        }));
    }

    protected void AddHoverOperationOnPossibleBlocks (TurnContext turnContext)
    {
        PossibleBlocks.ForEach(b => b.RuntimeBlock.CacheNotification());
        PossibleBlocks.ForEach(block => block.RuntimeBlock.SetHoverOperation(() =>
        {
            PossibleBlocks.ForEach(bd => bd.RuntimeBlock.LoadNotification());
            block.RuntimeBlock.Notify(RuntimeBlock.NotificationType.Healer);
            turnContext.FinalBlock = block;
        }));
    }
}