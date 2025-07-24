using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class OverTimeEffectData : StatusEffectData
{
    [field: TitleGroup("Settings")]
    [field: Unit(Units.Second)]
    [field: MinValue(.1f)]
    [field: SerializeField]
    public float IntervalBetweenExecution { get; private set; } = .5f;

    protected float timeFromLastEffect;

    public override IEnumerator Setup (StatusEffectContext ctx)
    {
        yield return base.Setup(ctx);

        timeFromLastEffect = 0f;
    }

    protected override IEnumerator Tick (BattleManager.TickContext ctx)
    {
        yield return base.Tick(ctx);

        timeFromLastEffect += ctx.deltaTime;

        if (timeFromLastEffect < IntervalBetweenExecution)
            yield break;

        Apply();
        timeFromLastEffect -= IntervalBetweenExecution;
    }

    protected abstract void Apply();
}