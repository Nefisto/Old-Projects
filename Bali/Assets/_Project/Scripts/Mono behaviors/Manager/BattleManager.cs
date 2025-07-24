using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Title("References")]
    [SerializeField]
    private Player player1;

    [SerializeField]
    private Player player2;

    [SerializeField]
    private WinnerPanel winnerPanel;

    [SerializeField]
    private FadeImage fadeBackground;

    [SerializeField]
    private LogPanel logPanel;
    
    private int currentPlayer;
    private Player CurrentPlayer => currentPlayer == 0 ? player1 : player2;
    private Player TargetPlayer => currentPlayer == 0 ? player2 : player1;

    private void Start()
        => BeginBattle();

    public void BeginBattle()
        => StartCoroutine(BattleRoutine());

    private IEnumerator BattleRoutine()
    {
        logPanel.ClearLogPanel();
        
        fadeBackground.SetColor(Color.black);
        winnerPanel.HidePanel();
        
        yield return player1.SetupPlayerSide("Nefisto");
        yield return player2.SetupPlayerSide("TsuDohNihm");
        
        player1.Shuffle();
        player2.Shuffle();

        yield return player1.DrawCardsIntoBackRowField();
        yield return player2.DrawCardsIntoBackRowField();

        yield return RandomizePlayer();

        yield return fadeBackground.FadeOut(1f);
        Utilities.CreateLog("Battle begin.", "Nefisto VS TsuDohNihm");

        yield return CurrentPlayer.SelectLeader();
        NextPlayer();
        yield return CurrentPlayer.SelectLeader();

        do
        {
            NextPlayer();

            var actionResult = new ActionResult();
            yield return CurrentPlayer.Act(actionResult);
            TargetPlayer.TakeDamage(actionResult.Damage);

            // Current player won
            if (TargetPlayer.HasDied())
                break;

            if (!TargetPlayer.CurrentLeader.IsAlive())
                yield return TargetPlayer.SelectLeader("Your leader has died, select a new one.");
        } while (true);
        
        winnerPanel.Show($"{CurrentPlayer.PlayerName} won!");
    }

    private void NextPlayer()
        => currentPlayer = (++currentPlayer) % 2;

    private IEnumerator RandomizePlayer()
    {
        yield return null;
        currentPlayer = Random.value < .5f ? 0 : 1;
    }
}

public class ActionResult
{
    public int Damage;
}