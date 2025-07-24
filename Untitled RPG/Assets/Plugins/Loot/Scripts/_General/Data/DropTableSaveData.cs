using System;
using System.Collections.Generic;
using Loot.Enum;

namespace Loot.Data
{
    [Serializable]
    public class DropTableSaveData
    {
        /// <summary>
        ///     Information to restore table when loading
        /// </summary>
        public string serializedReference;

        public PercentageCalculation percentageCalculation;
        public string description;

        public List<DropSaveData> dropSaveData;

        public DropTableSaveData (DropTable table, string serializedReference, Func<Drop, string> serializeDrop = null)
        {
            this.serializedReference = serializedReference;

            percentageCalculation = table.PercentageCalculation;
            description = table.Description;

            dropSaveData = new List<DropSaveData>();
            foreach (var drop in table.RawEnumerator())
            {
                var dropString = serializeDrop == null
                    ? string.Empty
                    : serializeDrop.Invoke(drop);

                dropSaveData.Add(new DropSaveData(drop, dropString));
            }
        }
    }
}