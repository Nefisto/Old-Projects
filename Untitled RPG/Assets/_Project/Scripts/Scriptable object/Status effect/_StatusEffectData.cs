using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class StatusEffectData : SerializedScriptableObject, IEquatable<StatusEffectData>
{
    public enum BuffOrDebuff
    {
        Buff,
        Debuff
    }

    public enum StatusEffectTarget
    {
        Target,
        Caster
    }

    [field: TitleGroup("Settings")]
    [field: MinValue(0f)]
    [field: SerializeField]
    public float Duration { get; protected set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public int Level { get; private set; } = 1;

    [field: TitleGroup("Settings")]
    [field: Tooltip("If true it will lock the battle during some initial animation runs")]
    [field: SerializeField]
    public bool ShouldRunAnimationAsync { get; private set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public BuffOrDebuff ConditionState { get; set; }

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public StatusEffectTarget EffectTarget { get; private set; }

    [TitleGroup("Debug")]
    [HideInEditorMode]
    [ReadOnly]
    [SerializeField]
    protected float timer;

    protected StatusEffectContext cachedContext;

    [TitleGroup("Settings")]
    [ShowInInspector]
    public abstract StatusEffectKind Kind { get; }

    [TitleGroup("Settings")]
    [ShowInInspector]
    public abstract string MessageOnScreen { get; }

    public StatusEffectData GetInstance => Instantiate(this);

    public bool ShouldBlockATB { get; protected set; } = false;

    public bool HasAlreadyStartedEndRoutine { get; private set; }

    public event Action<float> OnStatusEffectTick;
    public event Action OnStatusEffectEnd;

    /// <summary>
    ///     Called when applied
    /// </summary>
    public virtual IEnumerator Setup (StatusEffectContext ctx)
    {
        timer = Duration;
        cachedContext = ctx;

        BattleManager.battleTickEntryPoint += Tick;
        yield break;
    }

    protected virtual IEnumerator Tick (BattleManager.TickContext ctx)
    {
        if (timer <= 0)
        {
            yield return End();
            yield break;
        }

        timer -= ctx.deltaTime;
        RaiseStatusEffectTick(timer / Duration);
    }

    public virtual IEnumerator ApplyOnCurrentAction (ActionInfo actionInfo)
    {
        yield break;
    }

    public virtual IEnumerator End()
    {
        HasAlreadyStartedEndRoutine = true;

        BattleManager.battleTickEntryPoint -= Tick;
        OnStatusEffectEnd?.Invoke();

        yield break;
    }

    /// <summary>
    ///     Called when the status that we are applying was already applied
    /// </summary>
    public virtual IEnumerator Renew (StatusEffectContext ctx)
    {
        yield return null;
    }

    protected virtual void RaiseStatusEffectTick (float remainingPercentage)
        => OnStatusEffectTick?.Invoke(remainingPercentage);

    protected virtual void ShowEffectMessageOnScreen (StatusEffectContext ctx)
    {
        var floatTextSettings = new FloatTextSettings(MessageOnScreen, ctx.target.transform, textColor: Color.white);
        ServiceLocator.FloatText.AddCustomFloatText(floatTextSettings, 0);
    }
}