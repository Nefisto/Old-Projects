using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Loot
{
    /// <summary>
    ///     A small collapsed view of <see cref="Drop" />, this object is what you will find inside a <see cref="Bag" />.
    /// </summary>
    [Serializable]
    public class Loot
    {
        internal Loot (Drop drop)
        {
            Entry = drop.Entry;
            FromDrop = drop.OriginalDrop ?? drop;

            Amount = Random.Range(drop.AmountRange.x, drop.AmountRange.y + 1);
        }

        /// <summary>
        ///     A reference to original drop entry
        /// </summary>
        public ScriptableObject Entry { get; set; }

        /// <summary>
        ///     A reference to original drop, this can be used to change information on original drop when this entry is
        ///     dropped
        /// </summary>
        public Drop FromDrop { get; set; }

        /// <summary>
        ///     A randomized amount from original <see cref="Drop.AmountRange" />
        /// </summary>
        public int Amount { get; set; }
    }
}