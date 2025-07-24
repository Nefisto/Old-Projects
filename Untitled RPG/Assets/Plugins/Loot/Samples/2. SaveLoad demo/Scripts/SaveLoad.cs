using System.IO;
using Loot;
using Loot.Data;
using OldSample.Utilities;
using UnityEngine;

namespace OldSample.SaveLoad
{
    public class SaveLoad : MonoBehaviour
    {
        [Header("Ref")]
        public Operations tableOwner;

        public DropTableDrawer beforeTable;
        public DropTableDrawer afterTable;

        #region Save/Load examples

        /// <summary>
        ///     This will store only table current values, will work properly if you table only change their values and
        ///     not do any destructive/addictive operation at runtime like changing entry value, removing/adding new drops
        /// </summary>
        public void SimplestSave()
        {
            // Convert our table to save data
            var saveData = DropTable.Save(tableOwner.runtimeTable);

            // Create JSON
            var saveJson = JsonUtility.ToJson(saveData, true);

            // Then save it as you normally do when using things json
            using (var sw = new StreamWriter(Application.persistentDataPath + "/simplestSave.json"))
            {
                sw.Write(saveJson);
            }

            // If you don't want the json string to save, you can convert your table to a save data plain class 
            // var saveStructure = tableOwner.runtimeTable.ConvertToSaveData();
        }

        /// <summary>
        ///     IMPORTANT: Keep in mind that this version will just load values, this will not touch in modifiers/filters.
        /// </summary>
        public void SimplestLoad()
        {
            beforeTable.DrawDropTable(tableOwner.runtimeTable);

            // Get our saved string from file and remove the possible pretty print
            var savedJson = "";
            using (var sr = new StreamReader(Application.persistentDataPath + "/simplestSave.json"))
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                    savedJson += line;
            }

            // Load from json
            var loadedData = JsonUtility.FromJson<DropTableSaveData>(savedJson);

            // Here we will pass recovered data for our runtime table
            DropTable.Load(tableOwner.runtimeTable, loadedData);

            afterTable.DrawDropTable(tableOwner.runtimeTable);
        }

        /// <summary>
        ///     Here we are inserting methods to save table and entries
        /// </summary>
        public void ComplexSave()
        {
            // As this will suppose that destructive operations was done in our table, lets use a Save
            // override that allow us to recover table (for modifiers) and new entries
            var savedData = DropTable.Save(tableOwner.runtimeTable, new SaveData
            {
                serializeTable = () => tableOwner.templateTable.name, // A way to serialize our table (this will vary from system, here we are getting table using Resources.Load)
                serializeEntry = drop => drop.Entry == null ? string.Empty : drop.Entry.name // same as table
            });

            // Convert it to JSON
            var savedJson = JsonUtility.ToJson(savedData, true);

            // Then save it
            using (var sw = new StreamWriter(Application.persistentDataPath + "/complexSave.json"))
            {
                sw.Write(savedJson);
            }
        }

        /// <summary>
        ///     Here we recreate a clone table from ground, so you need to use information store in save to restore your original
        ///     table and entries
        /// </summary>
        public void ComplexLoad()
        {
            beforeTable.DrawDropTable(tableOwner.runtimeTable);

            // We will get our saved JSON from file
            var savedJson = "";
            using (var sr = new StreamReader(Application.persistentDataPath + "/complexSave.json"))
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                    savedJson += line;
            }

            // Create our data
            var savedData = JsonUtility.FromJson<DropTableSaveData>(savedJson);

            // Then we recovery our table
            // In this case our tables and items are being stored by resources
            tableOwner.runtimeTable = DropTable.Load(savedData, new LoadData
            {
                // REMEMBER: This step will vary from your own system, here we are getting this using the string to find the original table in resources
                deserializeTable = tableName => Resources
                    .Load<DropTable>("SaveLoad_Tables/" + tableName), // How to use string passed in save to restore the table? 

                deserializeEntry = entryName => string.IsNullOrWhiteSpace(entryName)
                    ? null
                    : Resources.Load<ScriptableObject>("SaveLoad_Items/" + entryName) // How to use string passed in save to restore each entry?
            });

            afterTable.DrawDropTable(tableOwner.runtimeTable);
        }

        #endregion
    }
}