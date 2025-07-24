using UnityEngine;

namespace Sample
{
    /*
     * This can be desirable approach to not allow player to reset game and try their luck again in same event
     * PS: Technically this isn't a behaviour of the system, its more like an guideline to how to attain this behaviour, because all that
     *  we are doing here is caching and using Random.state
     */
    public class Repeatable : MonoBehaviour
    {
        // [Header("Settings")]
        // public CaseData caseData;
        //
        // private Random.State cacheLastDrop;
        //
        // public void Drop()
        // {
        //     cacheLastDrop = Random.state;
        //
        //     var table = caseData.dropTable;
        //
        //     var bag = table.Drop();
        //
        //     Debug.Log($"Dropped items:");
        //     foreach (var loot in bag)
        //     {
        //         var item = (Item)loot.entry;
        //         
        //         item.Print(loot.Amount);
        //     }
        // }
        //
        // public void RepeatLast()
        // {
        //     Random.state = cacheLastDrop;
        //
        //     var table = caseData.dropTable;
        //
        //     // You only need to make sure that Random.state was set with desirable state
        //     var bag = table.Drop();
        //
        //     Debug.Log($"Repeating last drop:");
        //     foreach (var loot in bag)
        //     {
        //         var item = (Item)loot.entry;
        //         
        //         item.Print(loot.Amount);
        //     }
        // }
        //
        // protected override CaseData Data => caseData;
    }
}