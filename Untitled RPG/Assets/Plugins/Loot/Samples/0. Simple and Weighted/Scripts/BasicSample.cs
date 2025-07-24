using Sample;
using UnityEngine;

namespace OldSample
{
    /// <summary>
    ///     The normal case
    /// </summary>
    public class BasicSample : BaseCase
    {
        [Space]
        [Tooltip("Number of drops in multiple drop action")]
        [Header("Multiple drop settings")]
        public int numberOfDrops = 5;

        public void Drop()
        {
            // Every time that you call for a drop you will get a bag as a result
            var bag = dropTable.Drop();

            // Log results
            GameEvents.RaiseUpdateLog(new LogEntryContext { Label = $"You have dropped {bag.EntryCount} entries:" });

            if (bag.EntryCount == 0)
                return;

            // Bag is an IEnumerable<Loot>
            foreach (var loot in bag)
            {
                // Cast entry to our project type
                var item = (Item)loot.Entry;

                item.Print(loot.Amount);
            }
        }

        public void MultipleDrop()
        {
            // Getting our bag
            var bag = dropTable.Drop(numberOfDrops);

            // Log results
            GameEvents.RaiseUpdateLog(new LogEntryContext { Label = $"You have dropped {bag.EntryCount} entries:" });

            if (bag.EntryCount == 0)
                return;

            // OPTIONAL: As we possibly have some repetitions when using multiple drop, you can shrink your bag to group duplications
            bag.Shrink();

            // Bag is enumerable returning to us the loot version of each dropped item
            foreach (var loot in bag)
                // If we have only one type
                // var item = (Item)loot.entry;
                // Otherwise you must switch to verify each kind
                switch (loot.Entry)
                {
                    case Currency currency:

                        /* Do something with currency items here */
                        currency.Print(loot.Amount);
                        break;

                    case Misc misc:

                        /* Do something with misc items here */
                        misc.Print(loot.Amount);
                        break;

                    case Consumables consumables:

                        /* Do something with consumable items here */
                        consumables.Print(loot.Amount);
                        break;

                    default:
                        Debug.Log("We have dropped a type that don't have a case in our switch");
                        break;
                }
        }
    }
}