using NTools;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class Location
{
    [Button]
    private void Test_GetANewLocationModifier()
    {
        var task = new NTask(GetANewLocationModifier());
        task.OnFinished += _ => Refresh();
    }

    [Button]
    private void Test_UpdatingCurrentLocation() => StartCoroutine(UpdatingLocationRoutine(null));

    [Button]
    public static void Test_SetThisModifierToEveryLocation (LocationModifier locationModifier)
    {
        foreach (var location in AllLocations)
        {
            location.LocationModifier = new CurrentLocationModifier()
            {
                modifier = locationModifier,
                remainingBattles = 4
            };
            location.Refresh();
        }
    }

    [Button]
    public static void Test_GetANewModifierForAllLocations()
    {
        foreach (var location in AllLocations)
        {
            var task = new NTask(location.GetANewLocationModifier(), false);
            task.OnFinished += _ => location.Refresh();
            task.Start();
        }
    }
}
#endif