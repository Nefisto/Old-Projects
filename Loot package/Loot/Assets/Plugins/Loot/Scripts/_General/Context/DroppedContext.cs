using System;

// ReSharper disable UnusedMember.Global

namespace Loot.Context
{
    /// <summary>
    ///     Event called just after a drop has happened
    /// </summary>
    public class DroppedContext : EventArgs
    {
        /// <summary>
        ///     Bag with the drop results
        /// </summary>
        public Bag DroppedBag;

        /// <summary>
        ///     Table that triggered the drop
        /// </summary>
        public DropTable Table;

        public DroppedContext() { }

        public DroppedContext (DropTable table, Bag droppedBag)
        {
            Table = table;
            DroppedBag = droppedBag;
        }
    }
}