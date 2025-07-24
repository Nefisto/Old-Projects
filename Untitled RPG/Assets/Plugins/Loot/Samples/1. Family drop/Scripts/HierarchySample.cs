using System.Linq;
using Loot;

namespace Sample
{
    /*
     *  In some cases our drops follow an hierarch sequence so we can avoid the process of creating similar table just
     * inserting tables inside other tables and marking it as an extension of the current table. The sample below is
     * is what you will see in Hierarchy drop scene:
     *      - All wolfs { Fur, Potion }
     *      - Baby wolf { Common to wolves drops, Teeth }
     *      - Big wolf { Baby wolf drops, meat, sword A }
     *      - Mystical wolf { Big wolf drops, sword B }
     *      - Fenrir { Big wolf drops + Mystical wolf drops }
     *
     */
    public class HierarchySample : BaseCase
    {
        public void Drop()
        {
            // This is similar (if not equal) to simple/weight sample, the changes come from drop table on inspector only 
            var bag = dropTable.Drop().RerollAllTables();

            // Log results
            GameEvents.RaiseUpdateLog(new LogEntryContext { Label = $"You have dropped {bag.EntryCount} entries:" });

            foreach (var loot in bag)
            {
                var item = (Item)loot.Entry;

                item.Print(loot.Amount);
            }
        }

        // For this case I want to show all items from one bag before go to another bag
        public void StepByStep()
        {
            // This will make a basic drop, so our bag can have bags inside it
            var bag = dropTable.Drop();

            GameEvents.RaiseUpdateLog(new LogEntryContext { Label = $"Items dropped from {dropTable.name}" });

            // As we can have n layers of tables dropping tables here its a good idea to avoid repetition
            OpenLootThatCanBeAGift(bag);

            // Recursive method 
            void OpenLootThatCanBeAGift (Bag localBag)
            {
                // Remember that bag is just a IEnumerable<Loot>, so we can LINQ it
                var bagsInsideLocalBag = localBag
                    .Where(l => l.Entry is DropTable)
                    .ToList();

                // For now this is dropping only one item each time that we ask for a drop, but this can be easily changed in DropTable settings, so its probably a good idea to iterate over the bag
                foreach (var loot in localBag.Except(bagsInsideLocalBag))
                    // We need to switch entry to identify their type
                    switch (loot.Entry)
                    {
                        // We've dropped an item
                        case Item item:
                            item.Print(loot.Amount);
                            break;
                        // Other types...
                    }

                // After showing items that come from local bag, lets open the next bags
                foreach (var otherBag in bagsInsideLocalBag.Select(l => l.Entry as DropTable))
                {
                    GameEvents.RaiseUpdateLog(new LogEntryContext { Label = $"Items dropped from {otherBag.name}" });

                    OpenLootThatCanBeAGift(otherBag.Drop());
                }
            }
        }
    }
}