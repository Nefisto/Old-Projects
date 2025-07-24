using System;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable InconsistentNaming

public partial class GameEvents : ScriptableObject
{
    public static Action OnUpdateGameTries;

    public static Action onGameStart;

    public static Action onFinishedLoadingData;

    public static Action<Skill> onSelectSkill;
    public static Action OnPause;
    public static Action OnUnpause;

    public static Action OnTickEncounter;
    public static Action OpenCharacterCreationMenu { get; set; }

    public static EntryPoint<Template> OnBeginningAdventure { get; set; } = new();
    public static EntryPoint OnSetupAdventure { get; set; } = new();
    public static EntryPoint OnBegunAdventureEntryPoint { get; set; } = new();

    public static Action OnGameOverRetryButtonPressed { get; set; }

    public static Action OnGameOver { get; set; }

    [Button]
    private void ReloadAllData()
    {
        Database.UnloadAll();
        _ = new NTask(Database.LoadAll());
    }
}