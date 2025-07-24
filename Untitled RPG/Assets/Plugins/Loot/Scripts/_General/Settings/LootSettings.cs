using Loot.Utilities;
using UnityEngine;

// ReSharper disable Unity.RedundantSerializeFieldAttribute

namespace Loot
{
    public class LootSettings : InternalSingleton<LootSettings>
    {
        [SerializeField]
        private bool enableWorkOnOriginal;

        [SerializeField]
        private int maxDepthLayers = 7;

        [SerializeField]
        private int maxDropsPerCall = 10000;

        public static int MaxDropsPerCall
        {
            get => Instance.maxDropsPerCall;
            set => Instance.maxDropsPerCall = value;
        }

        // If you miss to clone a table before operate into it, the changes will be permanent, this options try to help you within it
        public static bool EnableWorkOnOriginal
        {
            get => Instance.enableWorkOnOriginal;
            set => Instance.enableWorkOnOriginal = value;
        }

        // When automatically rerolling table we must have an max depth level to avoid infinity reroll
        public static int MaxDepthLayers
        {
            get => Instance.maxDepthLayers;
            set => Instance.maxDepthLayers = value;
        }

        internal static void SetInstance (LootSettings loadedInstance)
            => _instance = loadedInstance;
    }
}