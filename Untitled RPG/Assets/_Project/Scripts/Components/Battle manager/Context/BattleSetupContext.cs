using System.Collections.Generic;
using NTools;

public class BattleSetupContext : IEntryPointContext
{
    public readonly bool autoStart = true;

    public List<EnemyData> enemiesData;
    public List<LocationModifier> locationModifiers = new();
}