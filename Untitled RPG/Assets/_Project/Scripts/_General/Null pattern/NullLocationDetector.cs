using System.Collections.Generic;
using NTools;

public class NullLocationDetector : ILocationDetector
{
    public LocationContext GetLocationContext()
        => new()
        {
            enemies = new List<EnemyData>
            {
                Database
                    .Enemies
                    .InstanceData
                    .GetRandom()
            }
        };
}