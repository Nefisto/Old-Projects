using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing over time", menuName = EditorConstants.MenuAssets.DOT_EFFECT + "Healing over time",
    order = 0)]
public class HealingOverTimeData : OverTimeEffectData
{
    [TitleGroup("Settings")]
    [SerializeField]
    private int healPerTick;

    [TitleGroup("Settings")]
    [Unit(Units.Second)]
    [MinValue(.1f)]
    [SerializeField]
    private float duration = .1f;

    [TitleGroup("Debug")]
    [ShowInInspector]
    public float TotalHealed => duration / IntervalBetweenExecution * healPerTick;

    public override StatusEffectKind Kind => StatusEffectKind.OvertimeHealing;
    public override string MessageOnScreen => "Heal";

    public override IEnumerator Renew (StatusEffectContext ctx)
    {
        timer = 0f;
        yield break;
    }

    protected override void Apply()
    {
        cachedContext.target.StartCoroutine(
            cachedContext.target.HealHealth(new ActionInfo() { flatHeal = healPerTick }));
    }
}