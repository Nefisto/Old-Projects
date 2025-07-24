#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER
using System.Linq;
using UnityEngine.UIElements;

namespace Loot.Editor
{
    public partial class LootDebugger
    {
        private ListView runtimeListView;

        private void RuntimeTableDrawer()
        {
            runtimeListView = rootVisualElement.Q<ListView>("runtime-table--list");
            runtimeListView.Clear();

            if (currentOriginalSelectedTable == null)
                return;

            currentOriginalSelectedTable.CleanClonesTable();

            runtimeListView.makeItem = () => new Label();
            runtimeListView.itemsSource = currentOriginalSelectedTable.clones;
            runtimeListView.bindItem = (item, i) =>
            {
                if (currentOriginalSelectedTable.clones[i] == null)
                    return;

                var currentCloneTable = currentOriginalSelectedTable.clones[i];
                var tableName = currentCloneTable.DebugName == ""
                    ? currentCloneTable.name
                    : currentCloneTable.DebugName;

                // Original table
                if (i == 0)
                    tableName += " (Original)";

                ((Label)item).text = tableName;
            };

            runtimeListView.selectedIndex = 0;
        }

        private void OnSelectionChangeInRuntimeListView()
            => runtimeListView.onSelectionChange += selectedObjects =>
            {
                cachedCurrentSelectedTableIndex = runtimeListView.selectedIndex;
                currentSelectedTable = (DropTable)selectedObjects.First();
                TableInformationDrawer();
            };
    }
}
#endif