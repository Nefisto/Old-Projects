#if UNITY_EDITOR
using Loot.Enum;
using UnityEditor;

namespace Loot.Editor
{
    [CustomEditor(typeof(DropTable))]
    public partial class DropTableEditor : UnityEditor.Editor
    {
        private const Filter FilterForUpdateWeightPercentageInEditor = Filter.IncludeExtensions | Filter.DontRemoveRepetitions | Filter.DontInvokeGlobalModify
                                                                       | Filter.DontInvokeGlobalModified | Filter.DontInvokeLocalModify
                                                                       | Filter.DontInvokeLocalModified | Filter.DontInvokeTableFilter;

        private SerializedProperty drops;
        private SerializedProperty percentageCalculation;
        private SerializedProperty tableDescription;

        private SerializedProperty tableIcon;
        private DropTable Target => (DropTable)target;

        private void OnEnable()
        {
            CacheProperties();
            SetupReorderableList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var sumOfWeightsOnThisRefresh = Target.InternalSumOfWeights();

            EditorGUILayout.PropertyField(tableIcon);
            EditorGUILayout.PropertyField(tableDescription);
            EditorGUILayout.PropertyField(percentageCalculation);
            if (Target.PercentageCalculation == PercentageCalculation.Weighted)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField("Total weight: \t", sumOfWeightsOnThisRefresh);
                EditorGUI.EndDisabledGroup();
            }

            reorderableList.DoLayoutList();

            if (Target.PercentageCalculation == PercentageCalculation.Weighted)
            {
                foreach (var drop in Target.CustomEnumerator(FilterForUpdateWeightPercentageInEditor, false))
                    drop.weightPercentage = drop.GetPercentage(sumOfWeightsOnThisRefresh);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void CacheProperties()
        {
            tableIcon = serializedObject.FindProperty(nameof(tableIcon));
            tableDescription = serializedObject.FindProperty(nameof(tableDescription));
            percentageCalculation = serializedObject.FindProperty(nameof(percentageCalculation));
            drops = serializedObject.FindProperty(nameof(drops));
        }
    }
}
#endif