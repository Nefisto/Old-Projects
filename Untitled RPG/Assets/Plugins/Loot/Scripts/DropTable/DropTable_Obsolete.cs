using System;

// ReSharper disable UnusedMember.Global

namespace Loot
{
    public sealed partial class DropTable
    {
        [Obsolete("Use Drop passing a parameter N instead")]
        public Bag MultipleDrop (int n)
            => Drop(n);

        [Obsolete("Use OnTableFilter instead")]
        public void AddFilter (Predicate<Drop> filter)
            => OnTableFilter += filter;
    }
}