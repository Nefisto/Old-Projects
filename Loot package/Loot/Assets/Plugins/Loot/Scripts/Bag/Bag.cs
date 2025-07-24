// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Loot
{
    /// <summary>
    ///     This is the result whenever you request a drop from any API thought drop table. Normally you will operate it
    ///     just as API results, but sometimes you want to manually keep adding drops inside a bag. It also implements an
    ///     IEnumerable&lt;Loot&gt; which means that you can use it in a LINQ query
    /// </summary>
    [Serializable]
    public partial class Bag : IEnumerable<Loot>, IEquatable<Bag>
    {
        /// <summary>
        ///     Used to verify if the re-roll is going further then max depth set in Loot settings
        /// </summary>
        private bool deeperThanMax;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Bag() { }

        /// <summary>
        ///     Convert the drop into loot and then create a bag with this loot
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Bag (Drop drop)
            => Add(drop);

        /// <summary>
        ///     Create a bag with that contains the IEnumerable&lt;Loot&gt; from the parameter
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Bag (IEnumerable<Loot> loot)
            => Loot = loot.ToList();

        /// <summary>
        ///     Convert each drop from the IEnumerable&lt;Drop&gt; to a loot and create a bag with those loots
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Bag (IEnumerable<Drop> drops)
            => Add(drops);

        /// <summary>
        ///     A list of selected drops, this is exposed thought a list of Loot class that is a minor view of the drop
        ///     that contains only the entry and a collapsed amount
        /// </summary>
        private List<Loot> Loot { get; } = new List<Loot>();
    }
}