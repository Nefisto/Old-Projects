using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class EnemyData : ActorData
{
    [field: HorizontalGroup("Settings/Info", 70)]
    [field: VerticalGroup("Settings/Info/Left", -5)]
    [field: TitleGroup("Settings")]
    [field: HideLabel]
    [field: PreviewField(ObjectFieldAlignment.Left)]
    [field: SerializeField]
    public Sprite Sprite { get; private set; }

    [field: HorizontalGroup("Settings/Info", 70)]
    [field: VerticalGroup("Settings/Info/Left", -5)]
    [field: TitleGroup("Settings")]
    [field: HideLabel]
    [field: PreviewField(ObjectFieldAlignment.Left)]
    [field: SerializeField]
    public override Sprite ATBIcon { get; protected set; }

    [field: HorizontalGroup("Settings/Info")]
    [field: VerticalGroup("Settings/Info/Right")]
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public EnemyBrain EnemyBrain { get; protected set; }

    [field: HorizontalGroup("Settings/Info")]
    [field: VerticalGroup("Settings/Info/Right")]
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    private MainTraitKind mainTraitKind;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public Skill DefaultAttackSkill { get; private set; }

    public override MainTraitKind MainTraitKind => mainTraitKind;
    public float InitialLifePercentage { get; set; } = 1f;

    [Button]
    private void ApplyAdvancementLevelToTrait (TraitChartFactory chartTemplate, float advancementLevel = 1)
    {
        var instance = chartTemplate.GetInstance();
        instance.MultiplyTraitChart(advancementLevel);
    }
}