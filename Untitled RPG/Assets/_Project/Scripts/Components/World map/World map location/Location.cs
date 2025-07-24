using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Loot;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public partial class Location : MonoBehaviour
{
    public static readonly List<Location> AllLocations = new();

    [field: TitleGroup("Settings")]
    [field: Min(1)]
    [field: SerializeField]
    public int LocationLevel { get; private set; } = 1;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    private CurrentLocationModifier LocationModifier { get; set; }

    [TitleGroup("Settings")]
    [SerializeField]
    private Vector2Int size = new(12, 3);

    [TitleGroup("References")]
    [SerializeField]
    private LocationLabel locationLabel;

    [TitleGroup("References")]
    [SerializeField]
    private BoxCollider2D boxCollider2D;

    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer border;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private DropTable possibleModifiersTable;

    private void Awake()
    {
        GameEvents.onFinishedLoadingData +=
            () => possibleModifiersTable = Database.LocationsTable;

        GameLoader.ThingsToLoadEntryPoint += () => UpdatingLocationRoutine(null);
        GameEvents.OnBattleFinishedEntryPoint += UpdatingLocationRoutine;
    }

    private void OnEnable() => AllLocations.Add(this);

    private void OnDisable() => AllLocations.Remove(this);

    private void OnValidate() => Refresh();

    public LocationContext GetLocationContext()
    {
        var enemies = Database
            .Enemies
            .Data
            .Where(ed => !ed.name.Contains("Default"))
            .Shuffle()
            .Take(Random.Range(1, 4))
            .Select(ed => ed.GetInstance())
            .ToList();

        enemies.ForEach(e => e.LevelUpTo(LocationLevel));

        return new LocationContext
        {
            enemies = enemies,
            locationModifier = LocationModifier.modifier
        };
    }

    private IEnumerator UpdatingLocationRoutine (BattleSetupContext _)
    {
        yield return UpdateLocationBonus();
        Refresh();
    }

    private IEnumerator UpdateLocationBonus()
    {
        if (!ShouldTryGetNewLocationBonus())
        {
            LocationModifier.remainingBattles--;
            yield break;
        }

        yield return GetANewLocationModifier();
    }

    private bool ShouldTryGetNewLocationBonus()
        => LocationModifier.modifier is null or EmptyModifier
           || LocationModifier.remainingBattles <= 1;

    private IEnumerator GetANewLocationModifier()
    {
        LocationModifier = new CurrentLocationModifier
        {
            remainingBattles = Random.Range(1, 4),
            modifier = (LocationModifier)possibleModifiersTable
                .DropAndRerollTables()
                .First()
                .Entry
        };

        yield break;
    }

    private void Refresh()
    {
        name = $"Location: lv {LocationLevel}";
        boxCollider2D.size = size;

        // Unity keeps send warning messages at every recompile about changing border size,
        //  adding this comparison hides it
        if (border.size != size)
            border.size = size;

        locationLabel.Refresh(new LocationLabelContext
        {
            level = LocationLevel,
            locationModifier = LocationModifier.modifier,
            battlesRemaining = LocationModifier.remainingBattles
        });
    }

    [Serializable]
    public class CurrentLocationModifier
    {
        [HideLabel]
        [HorizontalGroup]
        public LocationModifier modifier;

        [LabelText("Remaining:")]
        [HorizontalGroup]
        public int remainingBattles;
    }
}