using _Project.Scripts._General.Enum;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class Skill
{
    [TabGroup("Tab", "Animation", useFixedHeight: true)]
    [TitleGroup("Tab/Animation/Settings")]
    [SerializeField]
    private SkillAnimationType animationTypeEnum = SkillAnimationType.None;

    [field: TitleGroup("Tab/Animation/Variations")]
    [field: MinMaxSlider(.1f, 2f, showFields: true)]
    [field: SerializeField]
    protected Vector2 SpeedVariation { get; set; } = new(1f, 1f);

    [field: TitleGroup("Tab/Animation/Variations")]
    [field: MinMaxSlider(-1f, 1f, showFields: true)]
    [field: SerializeField]
    protected Vector2 OffsetVariationInX { get; set; }

    [field: TitleGroup("Tab/Animation/Variations")]
    [field: MinMaxSlider(-1f, 1f, showFields: true)]
    [field: SerializeField]
    protected Vector2 OffsetVariationInY { get; set; }

    [field: TitleGroup("Tab/Animation/Variations")]
    [field: MinMaxSlider(.5f, 1.5f, showFields: true)]
    [field: SerializeField]
    protected Vector2 SizeVariation { get; set; } = new(1f, 1f);

    [field: TitleGroup("Tab/Animation/Variations")]
    [field: SerializeField]
    protected bool CanFlipX { get; set; }

    [field: TitleGroup("Tab/Animation/Variations")]
    [field: SerializeField]
    protected bool ShouldFlipYForPlayer { get; set; }

    [TitleGroup("Tab/Animation/Settings")]
    [ShowInInspector]
    protected bool ShouldWaitForFinish
    {
        get => shouldWaitForFinish;
        set => shouldWaitForFinish = value;
    }

    public PlayAnimationSettings GetAnimationSettings (bool castedByEnemy)
        => new()
        {
            animationSpeed = SpeedVariation.GetRandom(),
            size = SizeVariation.GetRandom(),
            offsetX = OffsetVariationInX.GetRandom(),
            offsetY = OffsetVariationInY.GetRandom(),
            flipX = CanFlipX && Random.value > .5f,
            shouldFlipYForPlayer = ShouldFlipYForPlayer && castedByEnemy
        };

    [TitleGroup("Tab/Animation/Debug")]
    [DisableInPlayMode]
    [Button]
    private void CopyAnimationSettingsFrom (Skill skill)
    {
        SpeedVariation = skill.SpeedVariation;
        OffsetVariationInX = skill.OffsetVariationInX;
        OffsetVariationInY = skill.OffsetVariationInY;
        SizeVariation = skill.SizeVariation;
        CanFlipX = skill.CanFlipX;
        ShouldFlipYForPlayer = skill.ShouldFlipYForPlayer;
    }
}