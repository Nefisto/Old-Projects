using System.Linq;
using Loot;
using OldSample.Utilities;
using Sample;
using UnityEngine;

namespace OldSample.SaveLoad
{
    public class Operations : MonoBehaviour
    {
        [Header("Status")]
        public DropTable templateTable;

        public DropTable runtimeTable;

        // This item will be added at runtime
        [Header("Entry ref")]
        public Item elixirEntry;

        [Header("Ref")]
        public DropTableDrawer beforeTable;

        public DropTableDrawer afterTable;

        #region Support

        public void ResetTable()
        {
            runtimeTable = templateTable.Clone();

            beforeTable.DrawDropTable(runtimeTable);
            afterTable.DrawDropTable(runtimeTable);
        }

        #endregion

        #region Private Methods

        private void PrintBag (Bag bag)
        {
            Debug.Log($"Dropped {bag.EntryCount} items:");
            foreach (var loot in bag)
            {
                var item = loot.Entry as Item;

                Debug.Log(item ? $"{item.name}" : "Dropped an empty item");
            }
        }

        #endregion

        #region Monobehaviour callbacks

        private void Awake()
            // We will work with a temporary table
            => runtimeTable = templateTable.Clone();

        private void Start()
        {
            beforeTable.DrawDropTable(templateTable);
            afterTable.DrawDropTable(runtimeTable);
        }

        #endregion

        #region Simple operations

        // Simple operation example: Suppose that you don't want two have two sequential equal drops
        public void DropAndLock()
        {
            // Update the before panel on HUD
            beforeTable.DrawDropTable(runtimeTable);

            var bag = runtimeTable.CustomDrop(droppedCallback: context =>
            {
                // Remove lock from every item
                foreach (var drop in context.Table.RawEnumerator())
                    drop.IsDisabled = false;

                // Add lock to dropped items
                foreach (var loot in context.DroppedBag)
                    loot.FromDrop.IsDisabled = true;
            });

            PrintBag(bag);

            // Update the after panel on HUD
            afterTable.DrawDropTable(runtimeTable);
        }

        // A second simple example
        public void DropWithoutReplacement()
        {
            beforeTable.DrawDropTable(runtimeTable);

            // Here we are using weight as they were the amount of remaining drops, so a drop without replacement 
            // will just reduce the weight
            var bag = runtimeTable.CustomDrop(droppedCallback: context =>
            {
                foreach (var drop in context.DroppedBag.Select(loot => loot.FromDrop))
                {
                    if (drop.IsGuaranteed)
                        continue;

                    drop.Weight--;
                }
            });

            PrintBag(bag);

            afterTable.DrawDropTable(runtimeTable);
        }

        #endregion

        // By destructive I want to say that this will change entries in a way that we cannot recovery it from original table 

        #region Destructive operations

        // Adding drops in runtime
        public void AddElixir()
        {
            // beforeTable.DrawDropTable(runtimeTable);
            //
            // // A drop that will be added to the runtime table BUT does not exist in original table
            // var elixirDrop = new Drop()
            // {
            //     Entry = elixirEntry,
            //     AmountRange = new Vector2Int(1, 1),
            //     Weight = 1
            // };
            //
            // runtimeTable.AddDrop(elixirDrop);
            //
            // afterTable.DrawDropTable(runtimeTable);
        }

        // Changing entry value
        public void DropAndSetEmpty()
        {
            beforeTable.DrawDropTable(runtimeTable);

            var bag = runtimeTable.Drop();

            PrintBag(bag);

            // As we will set entry to null and print use entry name to log, this should happen after print
            foreach (var loot in bag)
                loot.FromDrop.Entry = null;

            afterTable.DrawDropTable(runtimeTable);
        }

        #endregion
    }
}