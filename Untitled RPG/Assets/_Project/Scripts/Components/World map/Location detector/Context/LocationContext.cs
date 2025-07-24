using System.Collections.Generic;

public struct LocationContext
{
    public List<EnemyData> enemies;
    public LocationModifier locationModifier;

    public List<EnemyData> GetEnemies()
        =>
            // Placeholder, later on this context will receive the drop table and the level and will calculate the 
            // correct enemies
            enemies;
}