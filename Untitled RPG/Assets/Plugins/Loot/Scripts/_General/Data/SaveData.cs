using System;

namespace Loot.Data
{
    /// <summary>
    ///     Used to serialize table in save operations
    /// </summary>
    public class SaveData
    {
        public Func<Drop, string> serializeEntry;
        public Func<string> serializeTable;

        public SaveData (Func<string> serializeTable = null, Func<Drop, string> serializeEntry = null)
        {
            this.serializeTable = serializeTable;
            this.serializeEntry = serializeEntry;
        }
    }
}