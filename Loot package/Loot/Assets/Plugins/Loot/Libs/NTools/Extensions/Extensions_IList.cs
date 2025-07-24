using System.Collections.Generic;

namespace Loot.NTools
{
    public static partial class Extensions
    {
        public static bool IsEmpty<T> (this IList<T> source)
            => source.Count == 0;
    }
}