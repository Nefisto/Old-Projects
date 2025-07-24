using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin master", menuName = EditorConstants.GOBLIN_PATH + "Goblin master")]
public class GoblinMaster : UnitData
{
    [TitleGroup("Settings")]
    [SerializeField]
    private UnitData goblinScullion;

    public override UnitSide UnitSide => UnitSide.Goblin;
    public override UnitKind UnitKind => UnitKind.GoblinMaster;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    protected override IEnumerator SummonStep (SummonStepSettings settings)
    {
        yield return SpawnRandomUnit();
        yield return base.SummonStep(settings);
    }

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var randomizedBlock = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .Shuffle()
            .FirstOrDefault(bd => bd.HasDwarfUnit);

        if (randomizedBlock is null)
            yield break;

        var cachedTargetData = randomizedBlock.UnitData;
        yield return randomizedBlock.ApplyDamage(new BlockData.ApplyDamageSettings() { damage = Damage });

        yield return CommonOperations.ProcessDeathAnimation();

        if (cachedTargetData.IsDead)
            yield return SpawnRandomUnit();
    }

    private IEnumerator SpawnRandomUnit()
    {
        var selectedBlock = CommonOperations
            .GetAllBlocksOnCurrentRoom()
            .Where(bd => !bd.HasUnitOnIt)
            .Shuffle()
            .FirstOrDefault();

        if (selectedBlock is null)
            yield break;

        var dataInstance = goblinScullion.GetInstance;

        yield return dataInstance.SummonUnit(new SummonStepSettings() { SummonedBlock = selectedBlock });
    }
}