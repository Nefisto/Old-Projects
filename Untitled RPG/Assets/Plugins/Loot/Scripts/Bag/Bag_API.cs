using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Loot.Utilities;

namespace Loot
{
    public partial class Bag
    {
        /// <summary>
        ///     Count unique entries inside the bag. Each unique entry count as 1, if you want the sum of amounts use
        ///     <see cref="AmountCount" />
        /// </summary>
        public int EntryCount => Loot.Count;

        /// <summary>
        ///     Give the sum of entry amounts. This will count the sum of each randomized entry amount, if you want just the
        ///     amount of unique entries, use <see cref="EntryCount" />
        /// </summary>
        public int AmountCount => Loot.Sum(l => l.Amount);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Bag RerollAllTables()
        {
            var cleanDrop = Reroll(Loot, 0);

            cleanDrop
                .RemoveAll(loot => loot.Entry is DropTable);

            if (deeperThanMax)
            {
                deeperThanMax = false;

                throw new WarningException(Messages.DeeperThanAllowedMaxDepth);
            }

            return new Bag(cleanDrop);
        }

        /// <summary>
        ///     Stack drops with the same entry reference
        /// </summary>
        /// <returns>New shrank bag</returns>
        public Bag Shrink()
        {
            var shrankBag = new Bag(this);

            for (var i = shrankBag.Loot.Count - 1; i >= 1; i--)
            {
                var loot = shrankBag.Loot[i];

                var foundAnotherCopy = false;
                for (var j = 0; j < i; j++)
                {
                    if (loot.Entry != shrankBag.Loot[j].Entry)
                        continue;

                    shrankBag.Loot[j].Amount += loot.Amount;
                    foundAnotherCopy = true;
                    break;
                }

                if (foundAnotherCopy)
                    shrankBag.Loot.Remove(loot);
            }

            return shrankBag;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Add (Drop drop)
        {
            var loot = new Loot(drop);
            Loot.Add(loot);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Add (Bag other)
            => Loot.AddRange(other.Loot);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Add (IEnumerable<Drop> guaranteedDrops)
        {
            foreach (var guaranteedDrop in guaranteedDrops)
                Add(guaranteedDrop);
        }

        private List<Loot> Reroll (List<Loot> loot, int depth)
        {
            // Stop going down
            if (depth >= LootSettings.MaxDepthLayers)
            {
                deeperThanMax = true;

                return loot;
            }

            var count = loot.Count;
            for (var i = 0; i < count; i++)
                if (loot[i].Entry is DropTable dt)
                {
                    var amount = loot[i].Amount;
                    while (amount-- > 0)
                    {
                        var bag = dt.Drop();
                        var newDrop = Reroll(bag.Loot, depth + 1);

                        loot.AddRange(newDrop);
                    }
                }

            return loot;
        }
    }
}