using System;
using UnityEngine;

namespace Loot.Data
{
    /// <summary>
    ///     Used to deserialize table in load operations
    /// </summary>
    public class LoadData
    {
        public Func<string, ScriptableObject> deserializeEntry;
        public Func<string, DropTable> deserializeTable;

        public LoadData (Func<string, DropTable> deserializeTable = null, Func<string, ScriptableObject> deserializeEntry = null)
        {
            this.deserializeTable = deserializeTable;
            this.deserializeEntry = deserializeEntry;
        }
    }
}