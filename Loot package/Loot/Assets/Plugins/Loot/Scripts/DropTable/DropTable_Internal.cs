using System.Collections.Generic;
using System.Linq;
using Loot.Context;
using Loot.Enum;
using Loot.NTools;

namespace Loot
{
    public sealed partial class DropTable
    {
        private const Filter FilterForCalculateSumOfWeightInEditor = Filter.DontRemoveRepetitions
                                                                     | Filter.DontInvokeGlobalModify | Filter.DontInvokeGlobalModified
                                                                     | Filter.DontInvokeLocalModify | Filter.DontInvokeLocalModified | Filter.DontInvokeTableFilter;

        internal void CleanClonesTable()
        {
            RemoveNullReferences();
            RemoveTablesWithSameReference();
            AddThisIfNotAlready();
        }

        internal int InternalSumOfWeights()
            => CustomEnumerator(FilterForCalculateSumOfWeightInEditor, false)
                .Where(IsValidToSumWeight)
                .Sum(drop => drop.Weight);

        private void RemoveNullReferences()
            => clones.RemoveAll(dt => dt == null);

        private void AddThisIfNotAlready()
        {
            if (!clones.Contains(this))
                clones.Add(this);
        }

        private void RemoveTablesWithSameReference()
            => clones = clones
                .Distinct(ReferenceEquals, table => table.GetHashCode())
                .ToList();

        internal void InvokeGlobalModify (IEnumerable<Drop> dropsCopy)
        {
            foreach (var drop in dropsCopy)
                OnGlobalModify?.Invoke(new ModifyContext(drop));
        }

        internal void InvokeGlobalModified (List<Drop> dropsCopy)
            => OnGlobalModified?.Invoke(new ModifiedContext(dropsCopy));

        internal void InvokeLocalModify (List<Drop> dropsCopy)
        {
            foreach (var drop in dropsCopy)
                OnLocalModify?.Invoke(new ModifyContext(drop));
        }

        internal void InvokeLocalModified (List<Drop> dropsCopy)
            => OnLocalModified?.Invoke(new ModifiedContext(dropsCopy));

        internal List<Drop> InvokeTableFilters (List<Drop> dropsCopy)
        {
            if (filters.Count == 0)
                return dropsCopy;

            return dropsCopy
                .Where(drop => filters.All(predicate => predicate.Invoke(drop)))
                .ToList();
        }
    }
}