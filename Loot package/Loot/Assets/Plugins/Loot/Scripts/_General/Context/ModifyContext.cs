using System;
using Loot;

// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Loot.Context
{
    public class ModifyContext : EventArgs
    {
        /// <summary>
        ///     The current drop on modify operation
        /// </summary>
        public Drop CurrentDrop;

        public ModifyContext (Drop currentDrop)
            => CurrentDrop = currentDrop;
    }
}