using System.Linq;
using Loot;
using Loot.Context;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class GlobalModifier : MonoBehaviour
    {
        [Header("Settings")]
        public Toggle refToDoubleChanceToggle;
        
        // DOC: Show that order matter here, if you toggle double chance, BApples will go from 20% to 70%, explain why
        private void Start()
        {
            
            // PS: Note that with this approach you cannot remove this rule, to remove it you need to clean global rules
            // RULE: Doubling all items percentage
            DropTable.OnGlobalModify += DoublePercentageWhenToggled;

            // If you want to remove rules at some point, you need to have FILTER and ACTION in methods
            // You can also add this from DropTable instance, 
            // RULE: Reducing consumables in 30%
            // DropTable.AddUniqueGlobalModifier(IsConsumable, Remove30Percent);
        }

        // This will run on each item
        private void DoublePercentageWhenToggled (ModifyContext args)
        {
            if (!refToDoubleChanceToggle.isOn)
                return;

            var drop = args.CurrentDrop;

            drop.Percentage *= 2;
        }

        private void ChangeConsumablePercentage (ModifyContext args)
        {
            var drop = args.CurrentDrop;

            if (!(drop.Entry is Consumables))
                return;

            drop.Percentage += 30f;
        }

        // This will run only 1 time after global modifiers finish
        private void RustSetBuff (ModifiedContext modifyEventArgs)
        {
            var drops = modifyEventArgs.ModifiedDrops;

            var rustList = drops
                .Where(x => x.Entry.name.ToUpper().Contains("RUST"))
                .ToList();
            
            // Apply buff only when we have more than 3 RUST items in our table
            if (rustList.Count <= 0)
                return;
            
            // Buff
            foreach (var drop in drops)
            {
                drop.Percentage += 15f;
                drop.AmountRange += new Vector2Int(1, 3);
            }
        }
        
        // You can also use UniqueRule API to make sure that you aren't adding a duplicate rule
        public void AddReducePercentageConsumables()
            => DropTable.OnGlobalModify += ChangeConsumablePercentage;

        public void RemoveReducePercentageConsumables()
            => DropTable.OnGlobalModify -= ChangeConsumablePercentage;
        
        // Note that both approaches work here, I'll use only A, but its only to illustrate the idea
        public void AddRustBuff()
            => DropTable.OnGlobalModified += RustSetBuff;
        
        public void RemoveRustBuff()
            => DropTable.OnGlobalModified -= RustSetBuff; 
    }
}