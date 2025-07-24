using System;
using System.Linq;
using Loot.Data;
using Loot.Enum;
using UnityEngine;

namespace Loot
{
    public partial class Drop
    {
        public ScriptableObject Entry
        {
            get => entry;
            set => entry = value;
        }

        public Vector2Int AmountRange
        {
            get => amountRange;
            set
            {
                if (value.x < amountLimit.x)
                    amountLimit.x = value.x;

                if (value.y > amountLimit.y)
                    amountLimit.y = value.y;

                if (value.x > value.y)
                    value.x = value.y;

                if (value.y < value.x)
                    value.y = value.x;

                amountRange = value;
            }
        }

        public int Weight
        {
            get => weight;
            set => weight = value;
        }

        /// <summary>
        ///     This will give you the percentage set on the inspector for simple tables, if you're looking
        ///     for the current percentage
        /// </summary>
        public float Percentage
        {
            get => odds;
            set => odds = value;
        }

        /// <summary>
        /// </summary>
        public bool IsExtensionDrop
        {
            get => isExtension;
            set => isExtension = value;
        }

        public bool IsGuaranteed
        {
            get => isGuaranteed;
            set => isGuaranteed = value;
        }

        public bool IsDisabled
        {
            get => isDisabled;
            set => isDisabled = value;
        }

        public event Action<Drop> Modifier;

        public Loot ToLoot() => new Loot(this);

        public float GetPercentage()
        {
            if (isExtension)
                return 100f;

            if (IsGuaranteed)
                return 100f;

            if (isDisabled)
                return 0f;

            if (ownerTable.PercentageCalculation == PercentageCalculation.Simple)
                return Percentage;

            var sumOfWeights = Mathf.Clamp(ownerTable.SumOfWeights(), 1, int.MaxValue);

            return (float)Weight / sumOfWeights * 100f;
        }

        public bool DeepEquals (Drop other)
            => Entry == other.Entry
               && AmountRange == other.AmountRange
               && Weight == other.Weight
               && Mathf.Abs(Percentage - other.Percentage) <= 0.001f
               && IsGuaranteed == other.isGuaranteed
               && IsDisabled == other.IsDisabled;

        public void LoadFromData (DropSaveData data, ScriptableObject loadedEntry = null)
        {
            if (loadedEntry)
                entry = loadedEntry;

            AmountRange = data.amountVariation;
            Weight = data.weight;
            Percentage = data.percentage;
            IsGuaranteed = data.isGuaranteed;
            IsDisabled = data.isHidden;

            data.isLoaded = true;
        }

        public void RaiseModifier() => Modifier?.Invoke(this);

        public bool Validate() => filters.Count == 0 || filters.Any(predicate => predicate.Invoke(this));

        public void AddFilter (Predicate<Drop> condition) => filters.Add(condition);

        public void ClearValidator() => filters.Clear();
    }
}