using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy AI", menuName = EditorConstants.ENEMY_AI + "Enemy AI", order = 0)]
public class SimpleEnemyAI : EnemyAI
{
    private const string ClashDescriptionTemplate = "CLASH: {0} at {1} with {2} at {3}";

    /// <summary>
    ///     \ => Disable this action totally
    ///     ++/-- => the amount of weight that this action gain/lost
    ///     . => Just if no other options are valid
    /// </summary>
    public override IEnumerator TurnHandle()
    {
        remainingActionPoints = ServiceLocator.GameContext.EnemyData.ActionPoints.MaxPoints;

        while (remainingActionPoints > 0)
        {
            CacheAllies();
            CacheEnemies();

            yield return GetPossibleActions();
            BigBrain();

            possibleActions = possibleActions
                .Where(e => e.actionCost <= remainingActionPoints)
                .OrderByDescending(a => a.priority)
                .ToList();

            yield return RunningActions();

#if UNITY_EDITOR
            ServiceLocator.GameplayMessage.UpdateSubMessage("Waiting for right mouse click");
            yield return new WaitForMouseDown();
#endif
        }
    }

    private void BigBrain()
    {
        // Check if together, units can focus their attack on a same unit to kill it
        var attackActionsSeparatedByCommonEnemy = possibleActions
            .Where(a => a is AttackAction)
            .Cast<AttackAction>()
            .GroupBy(a => a.targetBlock.UnitData);

        foreach (var group in attackActionsSeparatedByCommonEnemy)
        {
            var sumOfDamage = group.Sum(a => a.unitBlock.UnitData.Damage);

            if (sumOfDamage >= group.Key.CurrentHealth)
                group.ForEach(act => act.UpdatePriority(ActionPriorityUpdate.PositiveMedium));
        }
    }

    private IEnumerator GetPossibleActions()
    {
        possibleActions = new List<EnemyAction>();

        possibleActions.AddRange(GetMovementActions());
        yield return null;
        possibleActions.AddRange(GetAttackActions());
        yield return null;
        possibleActions.AddRange(GetSummoningActions());
    }

    /// <summary>
    ///     Summon actions
    ///     +++ Summoning can kill someone
    /// </summary>
    /// <returns></returns>
    private List<EnemyAction> GetSummoningActions()
    {
        var possibleSummonActions = new List<EnemyAction>();

        foreach (var card in ServiceLocator.GameContext.EnemyData.Hand)
        {
            if (card is not SummonCard summonCard)
                continue;

            // Get a relation between the position summon and 
            var positionToAction = ForeseeActionsOnPossiblePositions(summonCard.UnitData);

            var action = new SummonAction
            {
                description = $"Summon {summonCard.Name}",
                selectedCard = card,
                priority = 5,
                unitToSummon = summonCard.UnitData,
                actionCost = summonCard.Cost,
                positionToForeseeActions = positionToAction.ToNDictionary(pair => pair.Key,
                    pair => pair.Value)
            };

            action.Setup();
            possibleSummonActions.Add(action);
        }

        possibleSummonActions
            .Cast<SummonAction>()
            .ForEach(e => e.Sort());

        return possibleSummonActions;
    }

    /// <summary>
    ///     Attack actions
    ///     \ Don't have any enemy on attack range
    ///     ++ Can kill someone
    /// </summary>
    /// <returns></returns>
    private List<EnemyAction> GetAttackActions()
    {
        var possibleAttackActions = new List<EnemyAction>();

        foreach (var goblinBlock in blocksWithAllies)
        {
            if (!goblinBlock.UnitData.CanAttack)
                continue;

            var damage = goblinBlock.UnitData.Damage;

            var enemiesInView = goblinBlock.UnitData.GetEnemiesInView();

            foreach (var dwarf in enemiesInView)
            {
                var attackEntry = new AttackAction()
                {
                    description = "Attack",
                    unitBlock = goblinBlock,
                    targetBlock = dwarf.BlockData,
                    killTarget = damage >= dwarf.CurrentHealth
                };

                attackEntry.CalculatePriority();
                possibleAttackActions.Add(attackEntry);
            }
        }

        return possibleAttackActions;
    }

    /// <summary>
    ///     Movement actions
    ///     + if is not on a safe place
    ///     . if is on a safe place
    ///     ++ will die on next attack
    ///     +++ can move to a safe place
    ///     If a clash is possible
    ///     +++ if the clash will kill enemy but not me
    ///     + if clash will
    /// </summary>
    private List<EnemyAction> GetMovementActions()
    {
        var possibleMovementActions = new List<EnemyAction>();

        foreach (var goblinBlock in blocksWithAllies)
        {
            var goblinUnit = goblinBlock.UnitData;

            if (!goblinUnit.CanMove)
                continue;

            var freeMovements = GetAroundFreeMovements(goblinUnit);
            var clashMovements = GetClashMovements(goblinUnit);

            if (freeMovements.IsEmpty()
                && clashMovements.IsEmpty())
                continue;

            var currentPositionDangerLevel = GetDangerLevelOfPosition(goblinBlock.Position);

            foreach (var targetClashBlock in clashMovements)
            {
                var clashEntry = new ClashAction
                {
                    description = string.Format(ClashDescriptionTemplate, goblinUnit.Name,
                        goblinUnit.BlockData.Position, targetClashBlock.UnitData.Name, targetClashBlock.Position),
                    unitBlock = goblinBlock,
                    targetBlock = targetClashBlock,
                    willDie = goblinUnit.CurrentHealth == 1,
                    willKill = targetClashBlock.UnitData.CurrentHealth == 1 && targetClashBlock.UnitData.CanTakeDamage,
                    currentPositionDangerLevel = currentPositionDangerLevel,
                    targetPositionDangerLevel = GetDangerLevelOfPosition(targetClashBlock.Position)
                };

                clashEntry.CalculatePriority();
                possibleMovementActions.Add(clashEntry);
            }

            foreach (var targetBlock in freeMovements)
            {
                var freeMovementEntry = new MovementAction()
                {
                    description = $"Free move to {targetBlock.Position}",
                    unitBlock = goblinBlock,
                    targetBlock = targetBlock,
                    currentPositionDangerLevel = currentPositionDangerLevel,
                    targetPositionDangerLevel = GetDangerLevelOfPosition(targetBlock.Position)
                };

                freeMovementEntry.CalculatePriority();
                possibleMovementActions.Add(freeMovementEntry);
            }
        }

        return possibleMovementActions;
    }
}