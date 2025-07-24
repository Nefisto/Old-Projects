using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Percentage of spent mana",
    menuName = EditorConstants.MenuAssets.SPECIAL_RESOURCES + "Percentage of spent mana")]
public class PercentageOfSpentMana : SpecialResource
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public Gradient BarColor { get; private set; }

    [TitleGroup("Settings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float percentageFromMana;

    [TitleGroup("Settings")]
    [MinValue(0)]
    [SerializeField]
    private int maximum;

    [TitleGroup("Debug")]
    [ProgressBar(0, "@maximum", 0, 0, 1)]
    [HideInEditorMode]
    [SerializeField]
    private int current;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private GradientBar syncedBar;

    public override int Current
    {
        get => current;
        set
        {
            var oldValue = current;
            if (oldValue == value)
                return;

            current = value;
            syncedBar.UpdateBar(current / (float)maximum);

            RaiseUpdatedCurrent(oldValue, current);
        }
    }

    public override IEnumerator Setup (SetupSettings settings)
    {
        TurnController.OnRanAction += RanAction;

        syncedBar = settings.gradientBar;
        syncedBar.gameObject.SetActive(true);
        Current = 0;
        syncedBar.UpdateBar((float)Current / maximum);
        yield break;
    }

    private void RanAction (BattleActionContext ctx)
    {
        if (ctx.caster is not PlayerBattleActor)
            return;

        if (ctx.skill is JobSkill)
            return;

        Current = Mathf.Min(Mathf.RoundToInt(Current + ctx.skill.ResourceCost * percentageFromMana), maximum);
    }
}