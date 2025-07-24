using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private ActorTurnController playerController;

    [TitleGroup("References")]
    [SerializeField]
    private ActorTurnController enemyTurnController;

    public IEnumerator TurnHandle()
    {
        var gameContext = ServiceLocator.GameContext;

        ServiceLocator.GameplayMessage.UpdateMainMessage(
            $"Round: {gameContext.CurrentTurn} - {(gameContext.CurrentTurn % 2 != 0 ? "Player" : "Enemy")} turn");
        var currentActor = gameContext.CurrentTurn % 2 != 0 ? playerController : enemyTurnController;

        yield return currentActor.TurnHandle();
        gameContext.CurrentTurn++;
    }
}