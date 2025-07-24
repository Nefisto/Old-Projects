#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Loot.Editor
{
    public partial class LootDebugger
    {
        private const string OriginalTableListView = "original-table--list";

        private ListView originalListView;

        private void OriginalTableDrawer()
        {
            var tablesInProject = GetTablesInProject();

            originalListView = rootVisualElement.Q<ListView>(OriginalTableListView);

            originalListView.makeItem = () => new Label();
            originalListView.itemsSource = tablesInProject;

            originalListView.bindItem = (item, i) =>
            {
                ((Label)item).text = tablesInProject[i].name;
                ((Label)item).AddToClassList("Label");
            };
        }

        private void OnSelectionChangeInOriginalListView()
            => originalListView.onSelectionChange += selectedObjects =>
            {
                cachedOriginalSelectedTableIndex = originalListView.selectedIndex;
                currentOriginalSelectedTable = currentSelectedTable = (DropTable)selectedObjects.First();
                RuntimeTableDrawer();
            };

        private static List<DropTable> GetTablesInProject()
        {
            var tablesGuid = AssetDatabase.FindAssets("t:DropTable");
            var locations = tablesGuid
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => !path.StartsWith("Assets/Plugins/Loot"));

            return locations
                .Select(AssetDatabase.LoadAssetAtPath<DropTable>)
                .ToList();
        }
    }
}
#endif