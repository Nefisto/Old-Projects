using System;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GameplayIcons
{
    LevelUp
}

[CreateAssetMenu(fileName = "Game icons", menuName = EditorConstants.MenuAssets.DATABASE + "Game icons", order = 0)]
public partial class GameIcons : ScriptableObject
{
    [field: TabGroup("Tab", "Battle")]
    [field: PreviewField]
    [field: SerializeField]
    public Sprite PlayerAtbIcon { get; private set; }

    [field: TabGroup("Tab", "Battle")]
    [field: TitleGroup("Tab/Battle/Buff or debuff icon")]
    [field: HideReferenceObjectPicker]
    [field: SerializeField]
    public Icons<StatusEffectData.BuffOrDebuff> BuffOrDebuffIcons { get; private set; } = new();

    [field: TabGroup("Tab", "Status effect icons")]
    [field: HideReferenceObjectPicker]
    [field: HideLabel]
    [field: SerializeField]
    public Icons<StatusEffectKind> StatusEffectIcons { get; private set; } = new();

    [field: TabGroup("Tab", "Gameplay")]
    [field: HideReferenceObjectPicker]
    [field: HideLabel]
    [field: SerializeField]
    public Icons<GameplayIcons> GameplayIcons { get; private set; } = new();
}

[Serializable]
public class UnlockableIcons<T> where T : Enum
{
    [field: TitleGroup("Database")]
    [field: HideLabel]
    [field: SerializeField]
    public NDictionary<T, IconEntry> EnumToIcon { get; protected set; } = new();

    public Sprite GetIconOfKind (T kind) => EnumToIcon[kind].icon;

    [HideReferenceObjectPicker]
    [Serializable]
    public class IconEntry
    {
        [LabelWidth(100)]
        [HorizontalGroup]
        public bool isUnlocked;

        [HideLabel]
        [HorizontalGroup]
        [PreviewField]
        public Sprite icon;
    }
}

[Serializable]
public class Icons<T> where T : Enum
{
    [TitleGroup("Database", order: -1)]
    [InlineProperty]
    [PreviewField]
    [SerializeField]
    protected Sprite fallbackIcon;

    [TitleGroup("Database")]
    [HideLabel]
    [SerializeField]
    protected NDictionary<T, IconEntry> effectKindToSprite;

    public Sprite GetIconOfKind (T kind)
        => effectKindToSprite.ContainsKey(kind) ? effectKindToSprite[kind].icon : fallbackIcon;

    [HideReferenceObjectPicker]
    [Serializable]
    public class IconEntry
    {
        [HideLabel]
        [PreviewField]
        public Sprite icon;
    }
}