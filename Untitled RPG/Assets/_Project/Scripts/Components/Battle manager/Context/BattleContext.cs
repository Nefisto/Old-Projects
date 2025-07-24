using System.Collections.Generic;
using System.Linq;
using NTools;

public class BattleContext : IEntryPointContext
{
    public BattleActor Player { get; private set; }
    public List<BattleActor> Enemies { get; private set; }

    public IEnumerable<BattleActor> AllBattleActor
        => new List<BattleActor> { Player }
            .Concat(Enemies);

    public void AddPlayer (BattleActor player)
    {
        Player = player;
        Player.onDie += _ => Player = null;
    }

    public void AddEnemies (List<BattleActor> enemies)
    {
        Enemies = enemies.ToList();

        foreach (var enemy in Enemies)
            enemy.onLeaveCombat += () => Enemies.Remove(enemy);
    }
}