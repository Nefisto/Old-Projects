using System.Collections;
using NTools;
using UnityEngine;
using static IScreenFading;

public class GameLoader : MonoBehaviour
{
    public static EntryPoint ThingsToLoadEntryPoint = new();

    private IEnumerator Start()
    {
        yield return ServiceLocator.ScreenFading.FadeIn(new Settings()
        {
            duration = 0f
        });

        yield return Database.LoadAll();
        GameEvents.onFinishedLoadingData?.Invoke();
        yield return ThingsToLoadEntryPoint.YieldableInvoke();

        yield return ServiceLocator.ScreenFading.FadeOut(new Settings()
        {
            duration = .5f
        });

        GameEvents.onGameStart?.Invoke();
    }

    private void OnDestroy()
    {
        Database.UnloadAll();
    }
}