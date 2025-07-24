#if UNITY_EDITOR
using Loot.Enum;
using UnityEditor;
using UnityEngine;

namespace Loot.Editor
{
    [CustomPropertyDrawer(typeof(Drop))]
    public partial class DropDrawer : PropertyDrawer
    {
        // private const float WidthToVectorTakeMultipleLines = 345;

        private const float NumberOfLines = 6f;

        private readonly float percentageUsedToShowWeight = .7f;

        private readonly float spaceBetweenWeightValueAndLabel = 3f;

        public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            #region Control variables

            var currentPosition = rect.position;

            var oneLineRect = new Rect(rect.position, new Vector2(rect.width, EditorGUIUtility.singleLineHeight));

            var originalColor = GUI.color;
            var lowGrayColor = new Color(0.54f, 0.54f, 0.54f);

            #endregion

            #region Get fields

            entry = property.FindPropertyRelative(nameof(entry));
            amountRange = property.FindPropertyRelative(nameof(amountRange));
            amountLimit = property.FindPropertyRelative(nameof(amountLimit));
            weight = property.FindPropertyRelative(nameof(weight));
            odds = property.FindPropertyRelative(nameof(odds));
            isExtension = property.FindPropertyRelative(nameof(isExtension));
            isGuaranteed = property.FindPropertyRelative(nameof(isGuaranteed));
            weightPercentage = property.FindPropertyRelative(nameof(weightPercentage));
            isDisabled = property.FindPropertyRelative(nameof(isDisabled));
            ownerTable = property.FindPropertyRelative(nameof(ownerTable));
            hasAlreadyUpdatedLimit = property.FindPropertyRelative(nameof(hasAlreadyUpdatedLimit));
            var ownerTablePercentageCalculation = ((DropTable)ownerTable.objectReferenceValue)?.PercentageCalculation;
            var isEntryATableWithTheSameType = entry.objectReferenceValue is DropTable dt
                                               && dt.PercentageCalculation == ownerTablePercentageCalculation;

            #endregion

            #region Configuring Rects

            Rect entryRect = Rect.zero,
                amountVariationRect = Rect.zero,
                isGuaranteedRect = Rect.zero,
                isLockedRect = Rect.zero,
                weightRect = Rect.zero,
                weightPercentageRect = Rect.zero,
                percentageRect = Rect.zero;

            // Entry
            entryRect = new Rect(currentPosition, oneLineRect.size);
            currentPosition.y += entryRect.height + EditorGUIUtility.standardVerticalSpacing;

            // AmountVariation: This need additional height because vector will use one or two lines depending the amount width it have to show itself
            amountVariationRect = new Rect(currentPosition, new Vector2(oneLineRect.size.x, oneLineRect.size.y * 2f));
            currentPosition.y += amountVariationRect.height + EditorGUIUtility.standardVerticalSpacing;

            // Weight
            if (ownerTablePercentageCalculation == PercentageCalculation.Weighted)
            {
                weightRect = new Rect(currentPosition, new Vector2(oneLineRect.size.x * percentageUsedToShowWeight, oneLineRect.size.y));
                currentPosition.x += weightRect.width + spaceBetweenWeightValueAndLabel;
                weightPercentageRect = new Rect(currentPosition, new Vector2(oneLineRect.size.x * (1 - percentageUsedToShowWeight) - spaceBetweenWeightValueAndLabel, oneLineRect.size.y));
                currentPosition.x = rect.x;
                currentPosition.y += weightRect.height + EditorGUIUtility.standardVerticalSpacing;
            }
            // Simple 
            else if (ownerTablePercentageCalculation == PercentageCalculation.Simple)
            {
                percentageRect = new Rect(currentPosition, oneLineRect.size);
                currentPosition.y += percentageRect.height + EditorGUIUtility.standardVerticalSpacing;
            }

            // IsGuaranteed and IsLocked
            isGuaranteedRect = new Rect(currentPosition, new Vector2(oneLineRect.size.x * .5f, oneLineRect.size.y));
            currentPosition.x += isGuaranteedRect.width + spaceBetweenWeightValueAndLabel;
            isLockedRect = new Rect(currentPosition, oneLineRect.size);
            currentPosition.y += isLockedRect.height + EditorGUIUtility.standardVerticalSpacing;
            currentPosition.x = rect.x;

            // TreatAsTable
            var isExtensionDropRect = new Rect(currentPosition, oneLineRect.size);
            currentPosition.y += weightRect.height + EditorGUIUtility.standardVerticalSpacing;

            #endregion

            #region Draw

            EditorGUI.PropertyField(entryRect, entry, new GUIContent("Entry"));

            EditorGUI.BeginDisabledGroup(isExtension.boolValue);
            AmountVariationDraw(amountVariationRect, property, label);

            // Weight
            if (ownerTablePercentageCalculation == PercentageCalculation.Weighted)
            {
                EditorGUI.PropertyField(weightRect, weight, new GUIContent("Weight"));

                // Weight percentage
                var style = new GUIStyle
                {
                    normal = new GUIStyleState
                    {
                        textColor = Color.white,
                        background = Texture2D.normalTexture
                    },
                    alignment = TextAnchor.MiddleCenter
                };
                var prevFontColor = GUI.contentColor;
                GUI.contentColor = Color.white;
                GUI.backgroundColor = lowGrayColor;
                var weightPercentLabel = weightPercentage.floatValue > 0f
                    ? weightPercentage.floatValue.ToString("F")
                    : "--.--";
                GUI.Box(weightPercentageRect, weightPercentLabel, style);

                GUI.contentColor = prevFontColor;
                GUI.backgroundColor = originalColor;
            }
            else if (((DropTable)ownerTable.objectReferenceValue)?.PercentageCalculation == PercentageCalculation.Simple)
            {
                EditorGUI.PropertyField(percentageRect, odds, new GUIContent("Odds"));
            }

            // Is guaranteed and Is hidden
            EditorGUI.PropertyField(isGuaranteedRect, isGuaranteed, new GUIContent("Is guaranteed"));
            EditorGUI.PropertyField(isLockedRect, isDisabled, new GUIContent("Is disabled"));
            EditorGUI.EndDisabledGroup();

            // Is extension drop?
            EditorGUI.BeginDisabledGroup(!isEntryATableWithTheSameType);
            EditorGUI.PropertyField(isExtensionDropRect, isExtension, new GUIContent("Is extension drop?"));
            EditorGUI.EndDisabledGroup();
            if (!isEntryATableWithTheSameType)
                isExtension.boolValue = false;

            #endregion

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight * NumberOfLines + EditorGUIUtility.standardVerticalSpacing * NumberOfLines;

        #region Serialized properties

        private SerializedProperty entry;
        private SerializedProperty amountRange;
        private SerializedProperty amountLimit;
        private SerializedProperty weight;
        private SerializedProperty odds;
        private SerializedProperty isExtension;
        private SerializedProperty isGuaranteed;
        private SerializedProperty weightPercentage;
        private SerializedProperty isDisabled;

        private SerializedProperty ownerTable;

        // created the concept of limit later in project, so its used to manually update the old values to new
        private SerializedProperty hasAlreadyUpdatedLimit;

        #endregion
    }
}
#endif