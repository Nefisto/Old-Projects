using System.Collections.Generic;

namespace Loot.Context
{
    /// <summary>
    ///     Called just before the drop happens
    /// </summary>
    public class DroppingContext
    {
        /// <summary>
        ///     List of drops
        /// </summary>
        public List<Drop> Drops;

        /// <summary>
        ///     Table that triggered the drop
        /// </summary>
        public DropTable Table;

        public DroppingContext (DropTable table, List<Drop> drops)
        {
            Table = table;
            Drops = drops;
        }
    }
}