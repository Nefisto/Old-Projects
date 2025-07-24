using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public partial class StatusEffectController : SerializedMonoBehaviour, IEnumerable<StatusEffectData>
{
    [TitleGroup("References")]
    [SerializeField]
    private BattleActor battleActor;

    [TitleGroup("References")]
    [SerializeField]
    private ModifierIconController iconController;

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: SerializeField]
    public NDictionary<StatusEffectData, StatusEffectInstance> StatusEffectKindToTask { get; private set; }

    public float GetATBModifiers
        => StatusEffectKindToTask
            .Select(x => x.Value.instance)
            .Where(modifier => modifier is IATBModifier)
            .Sum(modifier => ((IATBModifier)modifier).Modifier);

    public event Action OnAddedEffect;
    public event Action OnRemovedEffect;

    public void Setup (BattleActor battleActor)
    {
        this.battleActor = battleActor;

        OnAddedEffect = null;
        OnRemovedEffect = null;
        StatusEffectKindToTask ??= new NDictionary<StatusEffectData, StatusEffectInstance>();
    }

    public IEnumerator ApplyStatusEffect (List<StatusEffectData> effectToApply)
    {
        foreach (var statusEffectData in effectToApply)
        {
            var ctx = new StatusEffectContext
            {
                target = battleActor,
                overrideData = statusEffectData
            };
            if (IsEffectAlreadyApplied(statusEffectData, out var statusEffectInstance))
            {
                yield return statusEffectInstance.instance.Renew(ctx);
                continue;
            }

            var instance = Instantiate(statusEffectData);
            battleActor.onDie += _ => TryRemoveEffect(instance);
            instance.OnStatusEffectEnd += () => TryRemoveEffect(instance);

            if (!instance.ShouldRunAnimationAsync)
                yield return instance.Setup(ctx);
            else
                StartCoroutine(instance.Setup(ctx));

            yield return iconController.CreateIcon(instance);

            StatusEffectKindToTask.Add(instance, new StatusEffectInstance { instance = instance });
            OnAddedEffect?.Invoke();
        }
    }

    private bool IsEffectAlreadyApplied (StatusEffectData statusEffectData,
        out StatusEffectInstance statusEffectInstance)
        => StatusEffectKindToTask.TryGetValue(statusEffectData, out statusEffectInstance);

    private bool TryRemoveEffect (StatusEffectData statusEffect)
    {
        if (!StatusEffectKindToTask.ContainsKey(statusEffect))
            return false;

        StatusEffectKindToTask.Remove(statusEffect);
        StartCoroutine(statusEffect.End());
        OnRemovedEffect?.Invoke();

        return true;
    }

    public IEnumerator CancelEffect (StatusEffectKind statusEffect)
    {
        var foundValue = StatusEffectKindToTask
            .First(x => x.Key.Kind == statusEffect);

        yield return foundValue.Value.instance.End();
    }

    public int GetLevelOf (StatusEffectKind kindOfEffect)
        => StatusEffectKindToTask
            .First(x => x.Key.Kind == kindOfEffect)
            .Value
            .instance
            .Level;

    /// <summary>
    ///     Return the amount of stacks if possible, otherwise returns 0
    /// </summary>
    public int GetAmountOfStacks (StatusEffectKind kindOfEffect)
        => StatusEffectKindToTask
            .First(x => x.Key.Kind == kindOfEffect)
            .Value
            .instance is IStackableStatusEffect stackableStatus
            ? stackableStatus.StackAmount
            : 0;

    public int AmountOf (StatusEffectData.BuffOrDebuff buffOrDebuff)
        => StatusEffectKindToTask
            .Count(t => t.Key.ConditionState == buffOrDebuff);

    [DisableInEditorButton]
    public bool HasCondition (StatusEffectKind statusEffect)
    {
        return StatusEffectKindToTask
                   .Keys
                   .FirstOrDefault(k => k.Kind == statusEffect)
               != null;
    }
}