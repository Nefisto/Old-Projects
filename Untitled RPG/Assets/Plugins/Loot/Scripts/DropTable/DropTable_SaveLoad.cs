using System.ComponentModel;
using System.Linq;
using Loot.Data;
using Loot.Utilities;

namespace Loot
{
    public sealed partial class DropTable
    {
        /// <summary>
        ///     Convert your table to a serializable structure that can be saved
        /// </summary>
        /// <remarks>
        ///     > [!Warning]
        ///     > Your modifiers and filters will not be saved
        /// </remarks>
        /// <example>
        ///     > [!Note]
        ///     > This example came from Save/Load demo scene with minor modification
        ///     <code language="cs">
        /// public void Save()
        /// {
        ///     // Convert our table to save data
        ///     var saveData = DropTable.Save(runtimeTable);
        /// 
        ///     // Create JSON
        ///     var saveJson = JsonUtility.ToJson(saveData, true);
        /// 
        ///     // Then save it as you normally do when using things json
        ///     using (var sw = new StreamWriter(Application.persistentDataPath + "/save.json"))
        ///     {
        ///         sw.Write(saveJson);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="tableToSerialize">Table that will be serialized</param>
        /// <returns>The structure ready to be saved in your disk</returns>
        // ReSharper disable once RedundantNameQualifier
        public static Data.DropTableSaveData Save (DropTable tableToSerialize)
            => new DropTableSaveData(tableToSerialize, string.Empty);

        /// <summary>
        ///     This override will receive a SaveParams to allow you to inform how we should serialize your
        ///     original table and/or drops, so you will be able to use this information to recovery table state in load methods
        /// </summary>
        /// <remarks>
        ///     > [!Warning]
        ///     > Your modifiers and filters will not be saved
        ///     > [!Note]
        ///     > Fields inside [SaveParams](xref:Loot.Params.SaveParams) are optional
        /// </remarks>
        /// <example>
        ///     > [!Note]
        ///     > This example came from Save/Load demo scene with minor modification
        ///     <code language="cs">
        /// public void Save()
        /// {
        ///     // As this will suppose that destructive operations was done in our table, lets use a Save
        ///     // override that allow us to recover table (for modifiers) and new entries
        ///     var savedData = DropTable.Save(runtimeTable, new SaveParams
        ///     {
        ///         serializeTable = () => templateTable.name, // Serialize our base table by name
        ///         serializeEntry = drop => drop.Entry == null  // Serialize each drop by name too
        ///                 ? string.Empty : drop.Entry.name
        ///     });
        ///     
        ///     // Convert it to JSON
        ///     var savedJson = JsonUtility.ToJson(savedData, true);
        /// 
        ///     // Then save it
        ///     using (var sw = new StreamWriter(Application.persistentDataPath + "/save.json"))
        ///     {
        ///         sw.Write(savedJson);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="tableToSerialize">Table that will be serialized</param>
        /// <param name="saveData">Class the contains your methods to transform table and drop into something serializable</param>
        /// <returns>The structure ready to be saved in your disk</returns>
        // ReSharper disable once RedundantNameQualifier
        public static Data.DropTableSaveData Save (DropTable tableToSerialize, SaveData saveData)
        {
            var tableString = saveData.serializeTable == null
                ? string.Empty
                : saveData.serializeTable.Invoke();

            return new DropTableSaveData(tableToSerialize, tableString, saveData.serializeEntry);
        }

        /// <summary>
        ///     This overload will load only values from your saved data, this means that addictive operations (drops that can't be
        ///     recovered
        ///     from your original table at runtime) will not be loaded
        /// </summary>
        /// <remarks>
        ///     > [!Warning]
        ///     > As remove drops isn't considered a destructive operation, this will compare entries in your data with drops in
        ///     your runtime table
        ///     > and if their entry name does not match drop will be removed
        /// </remarks>
        /// <example>
        ///     > [!Note]
        ///     > This example came from Save/Load demo scene with minor modification
        ///     <code language="cs">
        /// public void Load()
        /// {     
        ///     // Get our saved string from file and remove the possible pretty print
        ///     var savedJson = "";
        ///     using (var sr = new StreamReader(Application.persistentDataPath + "/simplestSave.json"))
        ///     {
        ///         var line = "";
        ///         while ((line = sr.ReadLine()) != null)
        ///             savedJson += line;
        ///     }
        ///  
        ///     // Load from json
        ///     var loadedData = JsonUtility.FromJson&lt;DropTableSaveData&gt;(savedJson);
        ///     
        ///     // Here we will pass recovered data for our runtime table
        ///     DropTable.Load(runtimeTable, loadedData);
        /// }
        /// </code>
        /// </example>
        /// <param name="runtimeTable">Drop table that will receive the loaded values</param>
        /// <param name="data">Deserialized data</param>
        /// <exception cref="WarningException">If you try to change values in a table that isn't a clone</exception>
        // ReSharper disable once RedundantNameQualifier
        public static void Load (DropTable runtimeTable, Data.DropTableSaveData data)
        {
            if (!runtimeTable.isClone)
                throw new WarningException(Messages.DestructiveOperationsOnOriginalAssetsWarning);

            runtimeTable.LoadFromData(data);
        }

        /// <summary>
        ///     This overload will use deserialize callbacks passed via loadParams to find and clone your table and create your
        ///     drops
        /// </summary>
        /// <remarks>
        ///     > [!Note]
        ///     > Your table will be cloned inside this method, so just pass a way to recovery original table
        /// </remarks>
        /// <example>
        ///     > [!Note]
        ///     > This example came from Save/Load demo scene with minor modification
        ///     <code language="cs">
        /// public void Load()
        /// {
        ///     // We will get our saved JSON from file and remove the possible pretty print
        ///     var savedJson = "";
        ///     using (var sr = new StreamReader(Application.persistentDataPath + "/save.json"))
        ///     {
        ///         var line = "";
        ///         while ((line = sr.ReadLine()) != null)
        ///             savedJson += line;
        ///     }
        /// 
        ///     // Create our data
        ///     var savedData = JsonUtility.FromJson&lt;DropTableSaveData&gt;(savedJson);
        ///     
        ///     // Then we recovery our table
        ///     // In this case our tables and items are being stored by resources
        ///     tableOwner.runtimeTable = DropTable.Load(savedData, new LoadParams 
        ///     {
        ///         deserializeTable = tableName => Resources
        ///             .Load&lt;DropTable&gt;("Tables/" + tableName),
        ///         
        ///         deserializeEntry = entryName => string.IsNullOrWhiteSpace(entryName) 
        ///             ? null 
        ///             : Resources.Load&lt;ScriptableObject&gt;("Items/" + entryName)
        ///     });
        /// }
        /// </code>
        /// </example>
        /// <param name="data">Deserialized data</param>
        /// <param name="loadData">Callbacks to recovery your table and drops</param>
        /// <returns>A clone of the table recovered by load params deserialize table</returns>
        // ReSharper disable once RedundantNameQualifier
        public static DropTable Load (Data.DropTableSaveData data, LoadData loadData)
        {
            // Clone our original table
            var table = loadData
                .deserializeTable
                .Invoke(data.serializedReference)
                .Clone();

            var deserializeEntry = loadData.deserializeEntry;

            // If we set a method to recovery entries, lets recreate them all
            if (deserializeEntry != null)
            {
                table.drops.Clear();

                foreach (var dropSaveData in data.dropSaveData)
                {
                    var entry = deserializeEntry.Invoke(dropSaveData.serializedReference);

                    var drop = new Drop();
                    drop.LoadFromData(dropSaveData, entry);

                    table.AddDrop(drop);
                }

                return table;
            }

            Load(table, data);

            return table;
        }

        private void LoadFromData (DropTableSaveData data)
        {
            percentageCalculation = data.percentageCalculation;
            tableDescription = data.description;

            for (var i = drops.Count - 1; i >= 0; i--)
            {
                var drop = drops[i];

                // Look for an item with the same entry name that isn't loaded yet
                var validData = data
                    .dropSaveData
                    .FirstOrDefault(dropData => !dropData.isLoaded
                                                && dropData.name == (drop.Entry == null ? "null" : drop.Entry.name));

                // If we don't find lets remove this drop
                if (validData == null)
                {
                    drops.Remove(drop);

                    continue;
                }

                drop.LoadFromData(validData);
            }
        }
    }
}