using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Loot.Enum;
using Loot.NTools;
using Loot.Utilities;
using UnityEngine;
using static Loot.Enum.Filter;

// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable UnusedMember.Global

namespace Loot
{
    public sealed partial class DropTable
    {
        public IEnumerator<Drop> GetEnumerator()
            => CustomEnumerator(Default).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <summary>
        ///     This enumerator will provide you the original drops that you see in the inspector, including hidden drops
        ///     without open the extensions and not applying any modify or filter.
        /// </summary>
        public IEnumerable<Drop> RawEnumerator()
            => CustomEnumerator(IncludeExtensions | IncludeDisabledDrops | DontRemoveRepetitions
                                | DontInvokeGlobalModify | DontInvokeGlobalModified
                                | DontInvokeLocalModify | DontInvokeLocalModified | DontInvokeTableFilter);

        /// <summary>
        ///     Here you can specify what kind of drops that you want to receive, take a look at <see cref="Filter" /> on
        ///     documentation
        /// </summary>
        public IEnumerable<Drop> CustomEnumerator (Filter settings)
            => CustomEnumerator(settings, true);

        internal IEnumerable<Drop> CustomEnumerator (Filter settings, bool cloneItems)
        {
            var flag = (int)settings;

            var includeExtensions = (flag & 1) == 0;
            flag >>= 1;
            var removeRepeatedExtensions = (flag & 1) == 0;
            var resultList = includeExtensions
                ? GetDropsIncludingExtensions(removeRepeatedExtensions)
                : drops;

            flag >>= 1;
            if ((flag & 1) == 0)
                resultList = RemoveHidden(resultList);

            if (cloneItems)
                resultList = CloneDrops(resultList);

            // We should not apply modifiers and filter when in editor
            if (!Application.isPlaying)
                return resultList;

            // As the collection must be collapsed to allow modifiers to apply, we do it just one time here
            var collapsedResult = resultList.ToList();
            flag >>= 1;
            if ((flag & 1) == 0)
                InvokeGlobalModify(collapsedResult);

            flag >>= 1;
            if ((flag & 1) == 0)
                InvokeGlobalModified(collapsedResult);

            flag >>= 1;
            if ((flag & 1) == 0)
                InvokeLocalModify(collapsedResult);

            flag >>= 1;
            if ((flag & 1) == 0)
                InvokeLocalModified(collapsedResult);

            flag >>= 1;
            if ((flag & 1) == 0)
                collapsedResult = InvokeTableFilters(collapsedResult);

            return collapsedResult;
        }

        private IEnumerable<Drop> GetDropsIncludingExtensions (bool excludeRepetitions)
        {
            var (resultList, extensionList) = SeparateNonExtensionsAndExtensions(drops);

            if (excludeRepetitions)
            {
                var comparer = new GenericComparer<DropTable>(ReferenceEquals, table => table.GetHashCode());
                extensionList = extensionList.Distinct(comparer).ToList();
            }

            var deepCounter = 0; // How many layer of extensions we already get down
            var currentLevelCount = extensionList.Count; // To mark when we entered in a new layer of extensions
            for (var i = 0; i < extensionList.Count; i++)
            {
                if (deepCounter == LootSettings.MaxDepthLayers)
                {
                    Debug.LogWarning(Messages.DeeperThanAllowedMaxDepth);
                    return resultList;
                }

                var (nonExtension, extension) = SeparateNonExtensionsAndExtensions(extensionList[i].drops);

                resultList = resultList.Concat(nonExtension);

                if (excludeRepetitions)
                    foreach (var dropTable in extension)
                    {
                        if (extensionList.Any(table => ReferenceEquals(table, dropTable)))
                            continue;

                        extensionList.Add(dropTable);
                    }
                else
                    extensionList.AddRange(extension);

                if (i != currentLevelCount)
                    continue;

                currentLevelCount = extensionList.Count;
                deepCounter++;
            }

            return resultList;
        }

        // Used for debugger

        /// <summary>
        ///     Get a list of tuples(A, B) where A is an extension drop and B is the amount of drops that exist
        ///     inside A that isnt another extension, including hidden (used to draw things in hierarchy on debugger)
        /// </summary>
        /// <returns></returns>
        internal List<(DropTable, int)> GetAllExtension()
        {
            var extensionList = new List<(DropTable table, int validDropAmount)> { (this, 0) };

            for (var i = 0; i < extensionList.Count; i++)
            {
                if (!extensionList[i].table.HasAnyExtensionDrop())
                    continue;

                var currentRoot = extensionList[i].table;
                var extensions = currentRoot
                    .CustomEnumerator(IncludeExtensions | IncludeDisabledDrops)
                    .Where(dt => dt.IsExtensionDrop)
                    .Cast<DropTable>();

                foreach (var dropTable in extensions)
                {
                    var validAmount = dropTable.RawEnumerator().Count(drop => !drop.IsExtensionDrop);
                    extensionList.Add((dropTable, validAmount));
                }
            }

            return extensionList
                .Skip(1)
                .ToList();
        }

        private bool HasAnyExtensionDrop()
            => this.Any(dt => dt.IsExtensionDrop);

        private static (IEnumerable<Drop> drops, List<DropTable> tables) SeparateNonExtensionsAndExtensions (IReadOnlyCollection<Drop> tableToSeparate)
            => (tableToSeparate
                    .Where(drop => !drop.InternalIsExtensionDrop),
                tableToSeparate
                    .Where(drop => drop.InternalIsExtensionDrop)
                    .Select(drop => drop.Entry as DropTable)
                    .ToList());

        private static IEnumerable<Drop> RemoveHidden (IEnumerable<Drop> innerDrops)
            => innerDrops.Where(d => !d.IsDisabled);

        private IEnumerable<Drop> CloneDrops (IEnumerable<Drop> innerDrops)
            => innerDrops
                .Select(d =>
                {
                    var clone = (Drop)d.Clone();
                    clone.ownerTable = this;

                    return clone;
                });
    }
}