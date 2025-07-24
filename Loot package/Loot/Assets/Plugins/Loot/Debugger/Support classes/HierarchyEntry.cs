#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER
using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
// Used to store information when showing entries throught the debug
    public class HierarchyEntry
    {
        public Drop DropWithoutModifiers;
        public Drop Drop;
        public List<DropTable> Hierarchy;
        public bool IsFilteredOut = false;
        public bool IsFirst;
        public int RepeatedLayer = -1;

        public HierarchyEntry (Drop drop, List<DropTable> hierarchy, int repeatedLayer = -1, bool isFirst = false)
        {
            DropWithoutModifiers = new Drop(drop, true);
            Drop = drop;
            Hierarchy = hierarchy;
            RepeatedLayer = repeatedLayer;
            IsFirst = isFirst;
        }

        public int HierarchyLevel => Mathf.Clamp(Hierarchy.Count - 1, 0, 10);

        public bool HasRepetition => RepeatedLayer != -1;
    }
}
#endif