using Loot;
using Loot.Context;
using UnityEngine;

namespace Sample
{
    /*
     * To allow us to remove modifiers, we need to add them based on a rule
     */
    public class ModifierApproachB : MonoBehaviour
    {
        // public CaseData data;
        //
        // public float increasePercentageIn = 10f;
        //
        // private void Start()
        // {
        //     var table = data.dropTable;
        //
        //     table.OnLocalModify += IncreaseConsumablePercentage; // This can be removed
        //     table.OnLocalModify += ctx => // This can't
        //     {
        //         var drop = ctx.currentDrop;
        //
        //         if (!(drop.Entry is Consumables))
        //             return;
        //
        //         drop.Percentage += increasePercentageIn;
        //     };
        // }
        //
        // private void IncreaseConsumablePercentage (ModifyContext ctx)
        // {
        //     var drop = ctx.currentDrop;
        //
        //     if (!(drop.Entry is Consumables))
        //         return;
        //
        //     drop.Percentage += increasePercentageIn;
        // }
        //
        // private void MultiplyConsumableAmount (ModifyContext ctx)
        // {
        //     var drop = ctx.currentDrop;
        //
        //     if (!(drop.Entry is Consumables))
        //         return;
        //
        //     drop.AmountRange *= 2;
        // }
        //
        // private void MakeConsumablesGuaranteed (ModifyContext ctx)
        // {
        //     var drop = ctx.currentDrop;
        //
        //     if (!(drop.Entry is Consumables))
        //         return;
        //
        //     drop.IsGuaranteed = true;
        // }
        //
        // #region Buttons
        //
        // public void AddMultiplyToConsumables()
        // {
        //     data.dropTable.OnLocalModify += MultiplyConsumableAmount;
        //     UpdateDropList();
        // }
        //
        // public void RemoveMultiplyToConsumables()
        // {
        //     data.dropTable.OnLocalModify -= MultiplyConsumableAmount;
        //     UpdateDropList();
        // }
        //
        // public void AddPercentToConsumables()
        // {
        //     data.dropTable.OnLocalModify += IncreaseConsumablePercentage;
        //     UpdateDropList();
        // }
        //
        // public void RemovePercentToConsumables()
        // {
        //     data.dropTable.OnLocalModify -= IncreaseConsumablePercentage;
        //     UpdateDropList();
        // }
        //
        // public void AddGuaranteedToConsumables()
        // {
        //     data.dropTable.OnLocalModify += MakeConsumablesGuaranteed;
        //     UpdateDropList();
        // }
        //
        // public void RemoveGuaranteedToConsumables()
        // {
        //     data.dropTable.OnLocalModify -= MakeConsumablesGuaranteed;
        //     UpdateDropList();
        // }
        //
        // #endregion
        //
        // protected override CaseData Data => data;
    }
}