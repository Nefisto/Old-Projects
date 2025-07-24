using System;
using System.Linq;
using Loot;
using Loot.Context;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class HiddenDrop : MonoBehaviour
    {
        // [Header("Settings")]
        // public CaseData caseData;
        //
        // [SerializeField] 
        // private Toggle waterHit;
        //
        // [SerializeField] 
        // private Toggle fireHit;
        //
        // /*
        //  * As we are going to modify a property of some specific items, lets find and add a modifier only to specific drops  
        //  */
        // private void Start()
        // {
        //     // Lets find the item that have water in name
        //     var waterDrop = caseData
        //         .dropTable
        //         .drops
        //         .FirstOrDefault(drop => drop.Entry.name.Contains("Water"));
        //     
        //     // This modifier action will be called when iterating over the table
        //     // this will make this drop isHidden to be in function of waterHit toggle
        //     if (waterDrop != null)
        //         waterDrop.Modifier += drop => drop.IsHidden = !waterHit.isOn;
        //
        //     // And now for fire
        //     var fireDrop = caseData
        //         .dropTable
        //         .drops
        //         .FirstOrDefault(drop => drop.Entry.name.Contains("Fire"));
        //
        //     if (fireDrop != null)
        //         fireDrop.Modifier += drop => drop.IsHidden = !fireHit.isOn;
        // }
        //
        // public void Drop()
        // {
        //     // Optional ref
        //     var table = caseData.dropTable;
        //     
        //     // As we have added modifiers to the specific drops, when we request a drop from the table things will just work
        //     var bag = table.Drop();
        //     
        //     // Print items
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