using System;
using System.Collections.Generic;
using System.Linq;

namespace Loot
{
    public static partial class Extensions
    {
        public static IEnumerable<T> Distinct<T> (this IEnumerable<T> source,
            Func<T, T, bool> equalityPredicate,
            Func<T, int> getHashMethod)
            => source.Distinct(new GenericComparer<T>(equalityPredicate, getHashMethod));
    }
}