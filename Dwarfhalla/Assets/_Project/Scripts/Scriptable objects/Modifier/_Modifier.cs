using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class Modifier : ScriptableObject, IEquatable<Modifier>
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public bool ShowIcon { get; private set; } = true;

    [TitleGroup("Settings")]
    [SerializeField]
    private Sprite icon;

    protected ModifierSettings cachedSettings;
    public Action OnRefresh;

    public Action OnApplied { get; set; }
    public EntryPoint<object> OnRemoved { get; set; } = new();

    public Modifier GetInstance => Instantiate(this);

    [TitleGroup("Settings")]
    [ShowInInspector]
    public abstract ModifierKind Kind { get; }

    public virtual bool CanBeApplied => true;

    public virtual Sprite GetIcon() => icon;

    public abstract IEnumerator Apply (ModifierSettings settings);

    public virtual IEnumerator Reapply (ModifierSettings settings)
    {
        yield break;
    }

    public virtual IEnumerator Remove()
    {
        yield return OnRemoved?.YieldableInvoke(this);
    }
}

public class ModifierSettings
{
    public UnitData caster;
    public Modifier modifier;
    public UnitData target;
}