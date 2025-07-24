using System.Collections.Generic;
using Loot;
using UnityEngine;
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace Sample
{
    public class TemporaryModifierB : MonoBehaviour
    {
        [Header("Settings")]
        public DropTable monsterTable;

        public void Drop()
        {
            // This one will overwrite common behavior making consumables to be guaranteed
            var bag = monsterTable.CustomDrop(droppingCallback: context =>
            {
                foreach (var drop in context.Drops)
                {
                    if (drop.Entry is Consumables)
                        drop.IsGuaranteed = true;
                }
            });
            
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
            foreach (var drop in monsterTable)
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