using System;
using System.Collections.Generic;

// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Loot.Context
{
    public class ModifiedContext : EventArgs
    {
        public List<Drop> ModifiedDrops;

        public ModifiedContext (List<Drop> modifiedDrops)
            => ModifiedDrops = modifiedDrops;
    }
}