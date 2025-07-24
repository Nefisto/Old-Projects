#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER
using System.Collections.Generic;
using System.Linq;
using Loot.Enum;
using UnityEngine;
using UnityEngine.UIElements;

namespace Loot.Editor
{
    public partial class LootDebugger
    {
        private readonly List<Color> repeatedBordersColors = new List<Color>
        {
            new Color(.52f, .36f, .36f, 1f),
            new Color(.36f, .52f, .36f, 1f),
            new Color(.36f, .36f, .52f, 1f),
            new Color(1f, .658823f, .501960f),
            new Color(.909803f, .752941f, .501960f),
            new Color(1f, .917664f, .501960f),
            new Color(.870588f, .960784f, .478431f)
        };

        private void TableInformationDrawer()
        {
            if (currentSelectedTable == null)
                return;

            var cachedSum = currentSelectedTable.SumOfWeights();

            UpdateTableIcon();
            UpdateHierarchyName();
            UpdatePercentageCalculation(cachedSum);

            var rateHeader = tableInformation.Q<Label>("table-information--drops--header--rate");
            rateHeader.text = currentSelectedTable.PercentageCalculation == PercentageCalculation.Simple ? "Odds" : "Weight";

            UpdateTableDescription();
            UpdateTableDrops(cachedSum);
        }

        private void UpdateTableDescription()
            => tableInformation.Q<Label>("table-information--description--value").text = currentSelectedTable.Description;

        private void UpdatePercentageCalculation (int cachedSum)
        {
            var percentageCalculation = currentSelectedTable.PercentageCalculation;
            tableInformation.Q<Label>("table-information--percentage-calculation--label").text = $"{percentageCalculation}";

            var totalWeight = tableInformation.Q<VisualElement>("table-information--percentage-calculation--total-weight");
            var isSimpleTable = percentageCalculation == PercentageCalculation.Simple;
            totalWeight.EnableInClassList("hide", isSimpleTable);

            if (!isSimpleTable)
                tableInformation.Q<Label>("table-information--percentage-calculation--total-weight--value").text = $"{cachedSum}";
        }

        private void UpdateHierarchyName()
        {
            var isOriginalCurrentSelected = ReferenceEquals(currentOriginalSelectedTable, currentSelectedTable);
            var clonedTableName = "Original";
            if (!isOriginalCurrentSelected)
                clonedTableName = currentSelectedTable.DebugName != "" ? currentSelectedTable.DebugName : currentSelectedTable.name;

            var tableNameAndInstance = $@"{currentOriginalSelectedTable.name}\{clonedTableName}";
            tableInformation.Q<Label>("table-information--name").text = tableNameAndInstance;
        }

        private void UpdateTableIcon()
        {
            var iconHUD = tableInformation.Q<VisualElement>("table-information--icon");

            iconHUD.style.backgroundImage = NoneIcon;

            if (currentSelectedTable.Icon == null)
                return;

            var texture = CreateTexture(currentSelectedTable.Icon);
            var croppedTexture = CropTextureFromSprite(texture);

            iconHUD.style.backgroundImage = croppedTexture;
        }

        private Texture2D CropTextureFromSprite (Texture2D texture)
        {
            var sprite = currentSelectedTable.Icon;
            var croppedTexture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
            var pixels = texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y,
                (int)sprite.textureRect.width, (int)sprite.textureRect.height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }

        /// <summary>
        ///     It is used to allow us to get access to the texture even when it isn't marked as read/write
        /// </summary>
        // Code adapted from: https://support.unity.com/hc/en-us/articles/206486626-How-can-I-get-pixels-from-unreadable-textures-
        private Texture2D CreateTexture (Sprite sprite)
        {
            var texture = sprite.texture;

            // Create a temporary RenderTexture of the same size as the texture
            var tmp = RenderTexture.GetTemporary(
                texture.width,
                texture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            // Blit the pixels on texture to the RenderTexture
            Graphics.Blit(texture, tmp);

            // Backup the currently set RenderTexture
            var previous = RenderTexture.active;

            // Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;

            // Create a new readable Texture2D to copy the pixels to it
            var myTexture2D = new Texture2D(texture.width, texture.height);

            // Copy the pixels from the RenderTexture to the new Texture
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();

            // Reset the active RenderTexture
            RenderTexture.active = previous;

            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);

            return myTexture2D;
        }

        private void UpdateTableDrops (int cachedSum)
        {
            var settings = new DrawEntrySettings
            {
                ShowHierarchy = showExtensions.value,
                ShowRepetition = showRepetition.value
            };

            var hierarchyList = GetDropsInHierarchyWay(currentSelectedTable, settings);
            var dropsListView = tableInformation.Q<ListView>("table-information--drops");

#if UNITY_2021_1_OR_NEWER
            dropsListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
#endif
            dropsListView.makeItem = () => DropEntry.Instantiate();
            dropsListView.itemsSource = hierarchyList;
            dropsListView.bindItem = (item, i) =>
            {
                if (i >= hierarchyList.Count)
                    return;

                var entryRoot = item.Q<VisualElement>("drop-list__entry");

                var entry = new DebugEntry(hierarchyList[i], currentSelectedTable.PercentageCalculation, cachedSum);

                entryRoot.style.backgroundColor = StyleKeyword.Null;
                entryRoot.EnableInClassList("table-list__entry--hierarchy", entry.IsExtension);
                entryRoot.EnableInClassList("table-list__entry--disabled", entry.IsDisabled);

                if (settings.ShowRepetition && hierarchyList[i].HasRepetition)
                {
                    if (!hierarchyList[i].IsFirst)
                        entry.Name += " -skipped-";

                    if (hierarchyList[i].RepeatedLayer < repeatedBordersColors.Count)
                    {
                        var borderColor = repeatedBordersColors[hierarchyList[i].RepeatedLayer];
                        entryRoot.style.backgroundColor = borderColor;
                    }
                }

                DrawEntry(entryRoot, entry, showColorTendency.value);
            };
        }

        private static void DrawEntry (VisualElement entryRoot, DebugEntry entry, bool shouldPaintTendency)
        {
            var nameLabel = entryRoot.Q<Label>("name");
            nameLabel.text = entry.Name;
            
            var guaranteedTag = entryRoot.Q<VisualElement>("tags--guaranteed");
            var disabledTag = entryRoot.Q<VisualElement>("tags--disabled");
            var extensionTag = entryRoot.Q<VisualElement>("tags--extension");
            var filteredTag = entryRoot.Q<VisualElement>("tags--filtered");
            guaranteedTag.EnableInClassList("occult", !entry.IsGuaranteed);
            disabledTag.EnableInClassList("occult", !entry.IsDisabled);
            extensionTag.EnableInClassList("occult", !entry.IsExtension);
            filteredTag.EnableInClassList("occult", !entry.IsFilteredOut);

            var rangeMin = entryRoot.Q<Label>("range__min");
            var rangeSeparator = entryRoot.Q<Label>("range__separator");
            var rangeMax = entryRoot.Q<Label>("range__max");
            rangeMin.text = entry.Minimum;
            rangeSeparator.text = entry.Separator;
            rangeMax.text = entry.Maximum;

            var weightLabel = entryRoot.Q<Label>("weight");
            weightLabel.text = entry.RateOrWeight;

            var finalPercentageLabel = entryRoot.Q<Label>("final-percentage");
            finalPercentageLabel.text = entry.FinalPercentage;

            if (!shouldPaintTendency)
                return;
            
            nameLabel.style.color = entry.NameColor;
            rangeMin.style.color = entry.MinimumColor;
            rangeMax.style.color = entry.MaximumColor;
            weightLabel.style.color = entry.RateOrWeightColor;
            finalPercentageLabel.style.color = entry.FinalPercentageColor;
        }

        private static List<HierarchyEntry> GetDropsInHierarchyWay (DropTable table, DrawEntrySettings drawEntrySettings)
        {
            var filter = drawEntrySettings.GetFilter();
            filter |= Filter.IncludeDisabledDrops
                      | Filter.DontInvokeGlobalModify
                      | Filter.DontInvokeGlobalModified
                      | Filter.DontInvokeLocalModify
                      | Filter.DontInvokeLocalModified
                      | Filter.DontInvokeTableFilter;

            var resultList = table
                .CustomEnumerator(filter)
                .Select(drop => new HierarchyEntry(drop, new List<DropTable> { table }))
                .ToList();

            var similarTableToIndexes = new Dictionary<DropTable, List<int>>();

            var deepCounter = 0;
            var nextLayerDeepCounter = resultList.Count;
            for (var i = 0; i < resultList.Count; i++)
            {
                if (i >= nextLayerDeepCounter)
                {
                    deepCounter++;
                    nextLayerDeepCounter = resultList.Count;
                }

                if (deepCounter >= LootSettings.MaxDepthLayers)
                    break;

                if (!(resultList[i].Drop.Entry is DropTable))
                    continue;

                var currentExtensionTable = (DropTable)resultList[i].Drop.Entry;

                if (similarTableToIndexes.ContainsKey(currentExtensionTable))
                {
                    similarTableToIndexes[currentExtensionTable].Add(i);
                    continue;
                }

                similarTableToIndexes.Add(currentExtensionTable, new List<int> { i });
                resultList[i].IsFirst = true;

                var newHierarchy = resultList[i].Hierarchy.ToList();
                newHierarchy.Add(currentExtensionTable);
                var innerDrops = currentExtensionTable
                    .CustomEnumerator(filter)
                    .Select(drop => new HierarchyEntry(drop, newHierarchy))
                    .ToList();

                resultList.InsertRange(i + 1, innerDrops);
            }

            if (Application.isPlaying)
            {
                var drops = resultList
                    .Where(he => !he.Drop.IsExtensionDrop)
                    .Select(he => he.Drop)
                    .ToList();
                table.InvokeGlobalModify(drops);
                table.InvokeGlobalModified(drops);
                table.InvokeLocalModify(drops);
                table.InvokeLocalModified(drops);

                var includedItems = table.InvokeTableFilters(drops);
                foreach (var hierarchyEntry in resultList)
                {
                    if (hierarchyEntry.Drop.IsExtensionDrop
                        || includedItems.Contains(hierarchyEntry.Drop))
                        continue;

                    hierarchyEntry.IsFilteredOut = true;
                }
            }

            foreach (var s in similarTableToIndexes.Where(tuple => tuple.Value.Count == 1).ToList())
                similarTableToIndexes.Remove(s.Key);

            var layer = 0;
            foreach (var indexes in similarTableToIndexes.Values)
            {
                foreach (var index in indexes)
                    resultList[index].RepeatedLayer = layer;

                layer++;
            }

            return resultList;
        }

        public class DrawEntrySettings
        {
            public bool ShowHierarchy;
            public bool ShowRepetition;

            public Filter GetFilter()
            {
                var filter = Filter.Default;

                if (ShowHierarchy)
                    filter |= Filter.IncludeExtensions;
                if (ShowRepetition)
                    filter |= Filter.DontRemoveRepetitions;

                return filter;
            }
        }
    }
}
#endif