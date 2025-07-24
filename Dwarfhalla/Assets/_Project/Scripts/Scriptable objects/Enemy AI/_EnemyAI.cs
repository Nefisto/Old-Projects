using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyAI : ScriptableObject
{
    protected List<BlockData> blocksWithAllies;
    protected List<BlockData> blocksWithEnemies;

    [TitleGroup("Debug")]
    [HideInEditorMode]
    [ShowInInspector]
    protected List<EnemyAction> possibleActions = new();

    [TitleGroup("Debug")]
    [ShowInInspector]
    protected int remainingActionPoints;

    public EnemyAI GetInstance => Instantiate(this);


    public abstract IEnumerator TurnHandle();

    protected IEnumerator RunningActions()
    {
        var selectedAction = possibleActions
            .GroupBy(e => e.priority)
            .OrderByDescending(e => e.Key)
            .FirstOrDefault()
            ?.GetRandom();

        if (selectedAction == null)
        {
            remainingActionPoints = 0;
            yield break;
        }

        remainingActionPoints -= selectedAction.actionCost;
        ServiceLocator.GameplayMessage.UpdateSubMessage($"{selectedAction.description}");
        yield return selectedAction switch
        {
            AttackAction attackAction => AttackActionRoutine(attackAction),
            SummonAction summonAction => SummonActionRoutine(summonAction),
            ClashAction clashAction => MovementActionRoutine(clashAction),
            MovementAction movementAction => MovementActionRoutine(movementAction),
            _ => throw new ArgumentOutOfRangeException()
        };

        yield return CommonOperations.ProcessDeathAnimation();

        CommonOperations.CancelNotificationOnAllGrid();
    }

    private IEnumerator AttackActionRoutine (AttackAction action)
    {
        var attacker = action.unitBlock.UnitData;

        action.unitBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.PossibleBlock);
        var previewTask = new NTask(attacker.PreviewAttack());
        yield return new WaitForSeconds(2f);
        previewTask.Stop();

        yield return attacker.PerformAttack();
    }

    private IEnumerator MovementActionRoutine (MovementAction action)
    {
        action.targetBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.PossibleBlock);
        action.unitBlock.RuntimeBlock.Notify(RuntimeBlock.NotificationType.PossibleBlock);
        yield return new WaitForSeconds(1.5f);
        yield return new Movement
        {
            InitialBlock = action.unitBlock,
            FinalBlock = action.targetBlock
        }.Run();

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator SummonActionRoutine (SummonAction action)
    {
        ServiceLocator.GameContext.EnemyData.RemoveCardFromHand(action.selectedCard);

        var instanceUnit = action.unitToSummon.GetInstance;
        var previewTask = new NTask(instanceUnit.PreviewAttack(new UnitData.PreviewSettings
        {
            CustomPosition = action.blockToSummon.Position
        }));
        action.blockToSummon.RuntimeBlock.Notify(RuntimeBlock.NotificationType.PossibleBlock);
        yield return new WaitForSeconds(1.5f);
        previewTask.Stop();

        yield return action.selectedCard.Perform(
            new SummonCard.SummonContext() { SelectedBlock = action.blockToSummon });
    }

    protected Dictionary<Vector2Int, ForeseeAction> ForeseeActionsOnPossiblePositions (UnitData unit)
    {
        var result = new Dictionary<Vector2Int, ForeseeAction>();

        foreach (var blockData in CommonOperations.GetAllFreeBlocksOnCurrentRoom())
        {
            var foreseeAction = unit.GetForeseeActions(blockData.Position);

            result.Add(blockData.Position, foreseeAction);
            foreseeAction.CalculatePriority();
        }

        return result;
    }

    protected List<BlockData> GetEnemiesThatCanView (Vector2Int position)
        => blocksWithEnemies
            .Select(bd => (bd, blocksInView: bd.UnitData.GetPositionInView()))
            .Where(entry => entry.blocksInView.Contains(position))
            .Select(entry => entry.bd)
            .ToList();

    protected List<BlockData> GetAroundFreeMovements (UnitData unit)
        => CommonOperations
            .GetAroundPatternPositionsFrom(unit.BlockData.Position)
            .Select(CommonOperations.GetBlockDataAt)
            .Where(bd => !bd.HasUnitOnIt)
            .ToList();

    // Danger level is how much damage the goblin will take staying on the position
    protected int GetDangerLevelOfPosition (Vector2Int originPosition)
    {
        var dangerLevel = CommonOperations
            .GetAroundPatternPositionsFrom(originPosition)
            .Select(CommonOperations.GetBlockDataAt)
            .Where(bd => bd.HasDwarfUnit)
            .Sum(bd => bd.UnitData.Damage);

        return dangerLevel;
    }

    protected List<BlockData> GetClashMovements (UnitData unit)
        => CommonOperations
            .GetAroundPatternPositionsFrom(unit.BlockData.Position)
            .Select(CommonOperations.GetBlockDataAt)
            .Where(bd => bd.HasDwarfUnit)
            .ToList();

    protected void CacheAllies()
        => blocksWithAllies = CommonOperations
            .GetAllBlocksOnCurrentRoom()
            .Where(b => b.HasGoblinUnit)
            .ToList();

    protected void CacheEnemies()
        => blocksWithEnemies = CommonOperations
            .GetAllBlocksOnCurrentRoom()
            .Where(b => b.HasDwarfUnit)
            .ToList();
}