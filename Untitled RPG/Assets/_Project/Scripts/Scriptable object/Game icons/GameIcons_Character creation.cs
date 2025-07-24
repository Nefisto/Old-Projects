using Sirenix.OdinInspector;
using UnityEngine;

public partial class GameIcons
{
    [field: TabGroup("Tab", "Character creation")]
    [field: HideReferenceObjectPicker]
    [field: HideLabel]
    [field: SerializeField]
    public Icons<CharacterCreationIcons> CharacterCreationIcons { get; private set; } = new();

    [field: TabGroup("Tab", "Character creation")]
    [field: HideLabel]
    [field: SerializeField]
    public UnlockableIcons<SaveSlotIconEnum> SaveSlotIcons { get; private set; } = new();
}

public enum SaveSlotIconEnum
{
    Bartz,
    BartzWarrior,
    BartzMonk,
    BartzThief,
    BartzDragon
}