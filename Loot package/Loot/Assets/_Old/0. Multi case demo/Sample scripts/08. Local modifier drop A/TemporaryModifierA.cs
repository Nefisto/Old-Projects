using System.Collections;
using System.Collections.Generic;
using Loot;
using Loot.Context;
using UnityEngine;

namespace Sample
{
    public class TemporaryModifierA : MonoBehaviour
    {
        [Header("Settings")]
        public DropTable table;
        
        [Range(0, 10)]
        public int angryLevel = 0;

        public float baseMultiplier = 5f;

        private DropTable runtimeTable;
        
        public IEnumerator Start()
        {
            yield return null; // Let LocalModifier apply things on table before we get a clone

            runtimeTable = table.Clone();
            
            // One simple and straight forward solution, is to apply local rules to an instance table
            // RULE: every angry level that this monster have will increase percentage in 5%
            runtimeTable.OnLocalModify += MultiplyByAngryLevel;
        }

        private void MultiplyByAngryLevel (ModifyContext ctx)
        {
            var drop = ctx.CurrentDrop;

            drop.Percentage += angryLevel * baseMultiplier;
        }

        public void Drop()
        {
            var bag = runtimeTable.Drop();
            
            foreach (var loot in bag)
            {
                var item = (Item)loot.Entry;
                
                item.Print(loot.Amount);
            }
        }

        public void OnHover()
        {
            // Items that will be showed in scroll view
            var items = new List<HUDScrollVIewItem>();

            // Table is enumerable by Drops
            foreach (var drop in runtimeTable)
            {
                // Create a new item to add in list
                var newItem = new HUDScrollVIewItem()
                {
                    Name = drop.Entry is null ? "Empty" : drop.Entry.name,
                    Amount = (drop.AmountRange.x, drop.AmountRange.y),
                    // Percentage = Table.CalculateDropPercentage(drop),
                    Percentage = drop.GetPercentage()
                };

                items.Add(newItem);
            }

            var scrollViewArguments = new UpdateScrollViewArguments
            {
                ItemsToShow = items
            };
            GameEvents.OnUpdateScrollView.Invoke(scrollViewArguments);
        }
    }
}