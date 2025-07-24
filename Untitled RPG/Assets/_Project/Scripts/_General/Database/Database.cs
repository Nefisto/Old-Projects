using System.Collections;
using System.Linq;
using Loot;

public static partial class Database
{
    public static FactoryDataLoader<ArmorDataFactory, ArmorData> Armors { get; } = new("ArmorData", true);

    public static FactoryDataLoader<WeaponDataFactory, WeaponData> Weapons { get; } =
        new("WeaponData", true);

    public static FactoryDataLoader<EnemyDataFactory, EnemyData> Enemies { get; } = new("EnemyData", true);

    public static FactoryDataLoader<PlayableCharacterDataFactory, PlayableCharacterData> PlayerData { get; } =
        new("PlayerData", true);

    public static DataLoader<Skill> Skills { get; } = new("SkillData", false);
    public static DataLoader<GameJob> GameJobs { get; } = new("Classes", true);
    public static DataLoader<LocationModifier> LocationsModifiers { get; } = new("LocationModifier", false);

    public static GameConstantsSO GameConstantsSo { get; private set; }
    public static GameIcons GameIcons { get; private set; }
    public static AccountData AccountData { get; private set; }
    public static DropTable LocationsTable { get; private set; }

    private static DataLoader<DropTable> LocationModifiersTable { get; } = new("LocationTable", false);
    private static DataLoader<GameIcons> LoadedBattleIcons { get; } = new("GameIcons", false);
    private static DataLoader<GameConstantsSO> LoadedGameSettings { get; } = new("GameSettings", false);
    private static DataLoader<AccountData> LoadedAccountData { get; } = new("AccountData", false);

    public static IEnumerator LoadAll()
    {
        yield return PlayerData.LoadData();
        yield return Enemies.LoadData();
        yield return Skills.LoadData();
        yield return GameJobs.LoadData();
        yield return Weapons.LoadData();
        yield return Armors.LoadData();
        yield return LocationsModifiers.LoadData();
        yield return LocationModifiersTable.LoadData(dt => LocationsTable = dt.First());

        yield return LoadedGameSettings.LoadData(data => GameConstantsSo = data.First());
        yield return LoadedBattleIcons.LoadData(data => GameIcons = data.First());
        yield return LoadedAccountData.LoadData(data => AccountData = data.First());

        yield return LoadServices();
    }

    public static void UnloadAll()
    {
        PlayerData.UnloadData();
        Enemies.UnloadData();
        Skills.UnloadData();
        GameJobs.UnloadData();
        Weapons.UnloadData();
        Armors.UnloadData();
        LocationsModifiers.UnloadData();
        LocationModifiersTable.UnloadData();

        LoadedGameSettings.UnloadData();
        LoadedBattleIcons.UnloadData();
        LoadedAccountData.UnloadData();

        UnloadServices();
    }
}