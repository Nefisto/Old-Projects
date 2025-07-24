using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class BattleManager
{
    [TabGroup("Turns")]
    [Title("Control")]
    [SerializeField]
    private BattleResultPanel resultPanel;
    
    [TabGroup("Turns")]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private List<BattleActor> turnOrder;
    
    private Task battleTask;
    
    public void BattleStart()
    {
        StartCoroutine(_BattleStart());

        IEnumerator _BattleStart()
        {
            yield return background.FadeOutRoutine(); 
                
            RecalculateTurnOrder();

            foreach (var battleActor in turnOrder)
            {
                yield return battleActor.BattleStart();
            }
            
            battleTask = new Task(BattleLoop());
        }
    }

    public void RemoveFromTurn (BattleActor actor)
    {
        if (!turnOrder.Contains(actor))
            return;

        turnOrder.Remove(actor);
    }
    
    private IEnumerator BattleLoop()
    {
        while (!IsBattleFinished())
        {
            yield return StartNewTurn();

            while (HasActorsThatDidntPlayedThisTurn())
            {
                var battleActor = GetNextActorInOrder();

                yield return battleActor.StartTurn();
                yield return battleActor.RunTurn();
                yield return battleActor.EndTurn();

                if (IsBattleFinished())
                    break;
            }
        }

        yield return resultPanel.ShowBattleResult(new BattleResultContext()
        {
            battleResult = battleResult
        });
    }

    private BattleActor GetNextActorInOrder()
    {
        var actor = turnOrder.First();
        turnOrder.Remove(actor);

        return actor;
    }

    private bool HasActorsThatDidntPlayedThisTurn()
        => turnOrder.Count != 0;

    private IEnumerator StartNewTurn()
    {
        RecalculateTurnOrder();

        yield return null;
    }

    private bool IsBattleFinished()
        => GetEnemiesCharacters().Count == 0;

    private void RecalculateTurnOrder()
    {
        turnOrder = GetAllTargets()
            .OrderByDescending(x => x.Data.Level)
            .ToList();
    }

#if UNITY_EDITOR
    public void EndTurnOfCurrentActor()
    {
        battleTask.Stop();
        battleTask.Start();
    }

    // This allow me to have control on turn reorder for debug reasons
    public void ManuallyInsertOrder (IEnumerable<BattleActor> newTurnOrder)
        => turnOrder = newTurnOrder.ToList();
#endif
}