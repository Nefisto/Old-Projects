using Loot;
using UnityEngine;

namespace Sample
{
    /// <summary>
    /// In this case we will remove entries from table after drop them, so the original table will work as a template
    /// to clone when current table get empty, or new instances are requested
    /// PS: System will try to prevent you from make permanent changes on your original table (without clone it first), you can remove this behaviour on global settings
    /// </summary>
    public class WithRemovalDrop : MonoBehaviour
    {
        // [Header("Settings")]
        // public CaseData caseData;
        //
        // private DropTable runtimeTable;
        //
        // private void Start()
        // {
        //     // Lets apply this to original table, so each clone table will have same callbacks
        //     // As we are removing drops here, this can mess things up if your global settings is set in a way that allow you to change values directly in assets
        //     caseData.dropTable.OnDrop += (sender, args) =>
        //     {
        //         var table = (DropTable)sender;
        //         var bag = args.droppedBag;
        //
        //         foreach (var loot in bag)
        //         {
        //             table.RemoveDrop(loot.fromDrop);
        //         }
        //     };
        //     
        //     GetANewCopy();
        // }
        //
        // public void Drop()
        // {
        //     // Here we want to remove drop after they get rolled
        //     var bag = runtimeTable.Drop();
        //     
        //     // If you try same thing on original table, this will throw an WarningException
        //     // var bag = caseData.dropTable.Drop();
        //
        //     foreach (var loot in bag)
        //     {
        //         var item = (Item)loot.entry;
        //
        //         item.Print(loot.Amount);
        //     }
        //
        //     if (runtimeTable.Count == 0)
        //     {
        //         Debug.Log($"Our table is empty, lets create a new one from template");
        //         GetANewCopy();
        //     }
        //
        //     UpdateDropList();
        // }
        //
        // private void GetANewCopy()
        // {
        //     runtimeTable = caseData.dropTable.Clone();
        // }
        //
        // protected override CaseData Data => caseData;
        // protected override DropTable Table => runtimeTable is null ? Data.dropTable : runtimeTable;
    }
}