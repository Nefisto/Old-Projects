using System.Linq;

namespace Loot
{
    public partial class Drop
    {
        public Drop() { }

        internal Drop (Drop originalDrop, bool isTemporary)
        {
            if (isTemporary)
            {
                // When object is temporary, it only exist in memory, so if a loot try to reference a temporary drop this can cause issues
                OriginalDrop = originalDrop.OriginalDrop == null
                    ? originalDrop : originalDrop.OriginalDrop;
            }

            Entry = originalDrop.Entry;
            ownerTable = originalDrop.ownerTable;
            AmountRange = originalDrop.AmountRange;
            Weight = originalDrop.Weight;
            Percentage = originalDrop.Percentage;
            IsExtensionDrop = originalDrop.IsExtensionDrop;
            IsGuaranteed = originalDrop.IsGuaranteed;
            IsDisabled = originalDrop.isDisabled;

            Modifier = originalDrop.Modifier;
            filters = originalDrop.filters.ToList();
        }

        public static Drop TemporaryDrop (Drop template)
            => new Drop(template, true);
    }
}