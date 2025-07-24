using Loot.Enum;
using UnityEngine;

namespace Loot
{
    public partial class Drop
    {
        /// <summary>
        ///     Used internally to update table TotalWeight and individually drop weight percentage on inspector
        /// </summary>
        internal float GetPercentage (int cachedSumOfWeights)
        {
            if (isExtension)
                return 100f;

            if (IsGuaranteed)
                return 100f;

            if (isDisabled)
                return 0f;

            if (ownerTable.PercentageCalculation == PercentageCalculation.Simple)
                return Percentage;

            var sumOfWeights = Mathf.Clamp(cachedSumOfWeights, 1, int.MaxValue);

            return (float)Weight / sumOfWeights * 100f;
        }
    }
}