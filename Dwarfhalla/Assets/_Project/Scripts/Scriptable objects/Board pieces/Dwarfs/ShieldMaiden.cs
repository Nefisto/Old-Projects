using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield maiden", menuName = EditorConstants.DWARF_PATH + "Shield maiden")]
public class ShieldMaiden : UnitData
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Modifier shieldModifier;

    [ShowInInspector]
    private List<Modifier> modifiersApplied = new();

    public override UnitSide UnitSide => UnitSide.Dwarf;
    public override UnitKind UnitKind => UnitKind.ShieldMaiden;

    public override Func<PatternGetSettings, IEnumerable<Vector2Int>> GetDamagePattern
        => CommonOperations.GetAroundPatternPositionsFrom;

    public override IEnumerator PerformAttack (PerformAttackSettings settings = null)
    {
        yield return base.PerformAttack(settings);

        CommonOperations.CancelNotificationOnAllGrid();
        TurnContext.HasSuccessfullyPerformedAction = true;

        var possibleBlocks = GetDamagePattern(new PatternGetSettings() { position = PiecePosition })
            .GetBlocksFromPositions()
            .ToList();

        var selectedEnemy = possibleBlocks
            .Where(bd => bd.HasGoblinUnit)
            .Shuffle()
            .FirstOrDefault();

        var allyToBuff = possibleBlocks
            .Where(b => b.HasDwarfUnit && !b.UnitData.HasModifier(ModifierKind.Shield))
            .Shuffle()
            .FirstOrDefault();

        if (selectedEnemy != null)
            yield return selectedEnemy.ApplyDamage(new BlockData.ApplyDamageSettings() { damage = Damage });

        if (allyToBuff != null)
        {
            var instanceModifier = shieldModifier.GetInstance;
            var target = allyToBuff.UnitData;

            target.OnDie += () => modifiersApplied.Remove(instanceModifier);
            instanceModifier.OnRemoved += _ => modifiersApplied.Remove(instanceModifier);

            modifiersApplied.Add(instanceModifier);
            yield return allyToBuff.ApplyModifier(new ModifierSettings
            {
                caster = this,
                target = target,
                modifier = instanceModifier
            });
        }
    }

    protected override IEnumerator Die()
    {
        foreach (var modifier in modifiersApplied.ToList())
            new NTask(modifier.Remove());

        modifiersApplied.Clear();

        yield return base.Die();
    }
}