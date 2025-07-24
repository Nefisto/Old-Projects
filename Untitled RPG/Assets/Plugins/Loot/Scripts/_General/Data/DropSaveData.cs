using System;
using UnityEngine;

namespace Loot.Data
{
    [Serializable]
    public class DropSaveData
    {
        /// <summary>
        ///     Information to restore drop when loading
        /// </summary>
        public string serializedReference;

        public string name;
        public Vector2Int amountVariation;
        public int weight;
        public float percentage;
        public bool isGuaranteed;
        public bool isHidden;

        [NonSerialized]
        public bool isLoaded;

        public DropSaveData (Drop drop, string serializedReference)
        {
            this.serializedReference = serializedReference;

            name = drop.Entry != null ? drop.Entry.name : "null";
            amountVariation = drop.AmountRange;
            weight = drop.Weight;
            percentage = drop.Percentage;
            isGuaranteed = drop.IsGuaranteed;
            isHidden = drop.IsDisabled;

            isLoaded = false;
        }
    }
}