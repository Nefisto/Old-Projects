using System.Linq;
using UnityEngine;

public class DebugFinishBattle : MonoBehaviour
{
    public void KillEnemies()
    {
        foreach (var enemy in ServiceLocator.BattleContext.Enemies.ToList())
        {
            StartCoroutine(enemy.TakePhysicalDamage(new ActionInfo { flatDamage = 10000 }));
        }
    }

    public void KillPlayer()
    {
        StartCoroutine(
            ServiceLocator.BattleContext.Player.TakePhysicalDamage(new ActionInfo() { flatDamage = 10000 }));
    }

    public void ReduceTriesToOne()
    {
        Blackboard.GameInfo.RemainingTries = 1;
        GameEvents.OnUpdateGameTries?.Invoke();
    }
}