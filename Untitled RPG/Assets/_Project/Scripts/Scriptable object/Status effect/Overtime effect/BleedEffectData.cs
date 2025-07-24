using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Bleed", menuName = EditorConstants.MenuAssets.DOT_EFFECT + "Bleed", order = 0)]
public class BleedEffectData : OverTimeEffectData
{
    [field: TitleGroup("Settings")]
    [field: MinValue(1)]
    [field: SerializeField]
    public int Damage { get; protected set; } = 3;

    [TitleGroup("Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float healingReduction;

    public override StatusEffectKind Kind => StatusEffectKind.Bleed;
    public override string MessageOnScreen => "Bleed";

    public override IEnumerator ApplyOnCurrentAction (ActionInfo actionInfo)
    {
        actionInfo.percentageOfFlatHealBonus.Add(("Bleed", -healingReduction));

        yield break;
    }

    protected override void Apply()
    {
        var target = cachedContext.target;
        var damageRoutine = target.TakeBleedDamage(Damage);
        target.StartCoroutine(damageRoutine);
    }
}