using System.Linq;
using Loot.Utilities;
using TMPro;
using UnityEngine;

namespace Sample
{
    /*
     * Filters can be applied in two levels, in a table level and in a drop level.
     * If you a filter in a drop level, only this drop will be validate under the filter but
     *      if you intend to add new drops to your time at runtime, you need to be sure to apply your
     *      custom filter each time.
     * If you apply your filter in a table level, every drop will be validate under it, this can
     *      cost have an extra performance cost but you don't need to worry after add or remove drops.
     * Here we will apply filters on both levels to illustrate.
     * Let's filter our table under those rules:
     *  - Each even attempt will show only consumable drops
     *  - Each third attempt will show only misc drops
     *  - otherwise all drops appear
     *
     * NOTE: If AT LEAST one filter returns true to a drop, it will appear in the list
     */
    public class FilterDrop : MonoBehaviour
    {
        // [Header("Settings")]
        // public CaseData caseData;
        //
        // [Header("Specific for case")]
        // public int attempt = 1; // for this case we want to do something after n attempts
        // public int nConsumables = 2;
        // public int nMisc = 3;
        // public TextMeshProUGUI buttonText; // Feedback for attempt number
        //
        // private void Start()
        // {
        //     // Lazy
        //     var table = caseData.dropTable;
        //     
        //     #region DROP LEVEL filter
        //
        //     // By default, all items appear, so this first filter is used to make sure that items that does now satisfy any condition when filtering will NOT appear
        //     // table.AddFilterToDrops(_ => true, // Every drop
        //     //     _ => attempt % nConsumables != 0 && attempt % nMisc != 0); // will appear when any special condition was being applied
        //     //
        //     // table.AddFilterToDrops(drop => drop.Entry is Consumables, // Only consumable
        //     //     _ => attempt % nConsumables == 0); // Will appear when their condition was true
        //     //
        //     // table.AddFilterToDrops(drop => drop.Entry is Misc, // Only misc 
        //     //     _ => attempt % nMisc == 0); // will appear when their condition was true
        //
        //     #endregion
        //
        //     #region TABLE LEVEL filter
        //
        //     // If any custom filter is being applying, show everything
        //     // table.AddFilter(_ => attempt % nConsumables != 0 && attempt % nMisc != 0);
        //     table.AddFilter(drop =>
        //     {
        //         if (!(drop.Entry is Consumables))
        //             return true;
        //     
        //         return attempt % nConsumables == 0;
        //     });
        //     
        //     table.AddFilter(drop =>
        //     {
        //         if (!(drop.Entry is Misc))
        //             return false;
        //     
        //         return attempt % nMisc == 0;
        //     });
        //
        //     #endregion
        //     
        //     // To increase attempt after each drop request to this table and repaint button
        //     table.OnDrop += (sender, context) =>
        //     {
        //         attempt++;
        //         buttonText.text = $"Drop - {attempt}";
        //     };
        // }
        //
        //
        // // After right setup in start, we don't need to do nothing special in drop
        // public void DropA()
        // {
        //     var bag = caseData.dropTable.Drop();
        //     
        //     foreach (var loot in bag)
        //     {
        //         var item = (Item)loot.entry;
        //
        //         item.Print(loot.Amount);
        //     }
        //     
        //     UpdateDropList();
        // }
        //
        // protected override CaseData Data => caseData;
    }
}