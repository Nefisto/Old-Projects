using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Movement
{
    public BlockData InitialBlock { get; set; }
    public BlockData FinalBlock { get; set; }

    public IEnumerator Run()
    {
        var caster = InitialBlock.UnitData;
        yield return FinalBlock.HasUnitOnIt ? ClashMovement() : NormalMovement();
        caster.ConsumeMovement();
    }

    private IEnumerator NormalMovement()
    {
        var instanceUnit = InitialBlock.UnitData.RuntimeUnit;

        var moveDuration = Vector3
                               .Distance(InitialBlock.RuntimeBlock.PiecePosition.position,
                                   FinalBlock.RuntimeBlock.PiecePosition.position)
                           / 5f;
        var tween = instanceUnit
            .transform
            .DOMove(FinalBlock.RuntimeBlock.PiecePosition.position, moveDuration)
            .SetEase(Ease.Linear);

        yield return tween.WaitForCompletion();

        FinalBlock.SetUnit(InitialBlock.UnitData);
        InitialBlock.RemoveUnit();
    }

    private IEnumerator ClashMovement()
    {
        var moveDuration = Vector3
                               .Distance(InitialBlock.RuntimeBlock.PiecePosition.position,
                                   FinalBlock.RuntimeBlock.PiecePosition.position)
                           / 5f;

        var casterData = InitialBlock.UnitData;
        var targetData = FinalBlock.UnitData;
        var direction = InitialBlock
            .Position
            .GetDirectionTo(FinalBlock.Position)
            .ToXZY();
        var halfDirection = direction * .5f;

        var casterMovementTween = casterData
            .RuntimeUnit
            .transform
            .DOMove(FinalBlock.RuntimeBlock.PiecePosition.position - halfDirection, moveDuration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false);

        var targetMovementTween = targetData
            .RuntimeUnit
            .transform
            .DOMove(FinalBlock.RuntimeBlock.PiecePosition.position + halfDirection, moveDuration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false);

        yield return casterMovementTween.WaitForCompletion();
        yield return targetMovementTween.WaitForCompletion();
        // Normally I don't do this part, but dotween was sending a warning about tween get killed and being invalid, so
        //  I just disable the auto kill and it seems to get fixed
        casterMovementTween.Kill();
        targetMovementTween.Kill();

        yield return InitialBlock.ApplyDamage(new BlockData.ApplyDamageSettings() { damage = 1 });
        yield return FinalBlock.ApplyDamage(new BlockData.ApplyDamageSettings() { damage = 1 });

        if (!targetData.IsDead)
        {
            var adjustTargetPosition = targetData
                .RuntimeUnit
                .transform
                .DOMove(FinalBlock.RuntimeBlock.PiecePosition.position, moveDuration)
                .SetEase(Ease.Linear);
            yield return adjustTargetPosition.WaitForCompletion();
        }

        if (targetData.IsDead && !casterData.IsDead)
            yield return casterData.OnKillBehavior();

        if (casterData.IsDead)
        {
            if (!targetData.IsDead)
                yield return targetData.OnKillBehavior();

            yield break;
        }

        var correctBlock = targetData.IsDead
            ? FinalBlock
            : CommonOperations.GetBlockDataAt(new Vector2Int((int)(FinalBlock.Position.x - direction.x),
                (int)(FinalBlock.Position.y - direction.z)));

        var adjustCasterPosition = casterData
            .RuntimeUnit
            .transform
            .DOMove(correctBlock.RuntimeBlock.PiecePosition.position, moveDuration)
            .SetEase(Ease.Linear);

        yield return adjustCasterPosition.WaitForCompletion();

        InitialBlock.RemoveUnit();
        correctBlock.SetUnit(casterData);
    }
}