using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TurnController turnController;

    private NTask gameRoutine;
    public static List<UnitData> DeadUnits { get; private set; } = new();

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);

        gameRoutine = new NTask(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        Medic.CachedHealing = new Dictionary<UnitData, int>();

        // Fade in
        ServiceLocator.GameContext = new GameContext();
        yield return GameEntryPoints.OnLoadAssets?.YieldableInvoke();
        yield return GameEntryPoints.GeneratingSessionData?.YieldableInvoke();
        yield return GameEntryPoints.OnSetupScene?.YieldableInvoke();
        yield return ServiceLocator.ScreenFading.FadeOut(new ScreenFading.Settings { duration = 1f });
        yield return GameEntryPoints.OnRenderingLevel?.YieldableInvoke();
        yield return GameEntryPoints.OnFinishedSetup?.YieldableInvoke();

        while (true)
        {
            // Temporary, because atm player can interact with any piece, this can cause issues with pieces visual
            CommonOperations
                .GetAllBlocksOnCurrentRoom()
                .Where(bd => bd.HasUnitOnIt)
                .ForEach(bd => StartCoroutine(bd.UnitData.RuntimeUnit.EnablePieceAttack()));

            yield return turnController.TurnHandle();
            yield return CommonOperations.ProcessDeathAnimation();

            yield return null;
        }
    }
}