#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Loot.Editor
{
    public partial class DropTableEditor
    {
        private readonly Color isActiveColor = new Color(0.173f, 0.365f, 0.529f, 1f); // A tone of blue
        private readonly Color isEvenColor = new Color(0.235f, 0.235f, 0.235f, 1f); // Brighter gray
        private readonly Color isOddColor = new Color(0.2f, 0.2f, 0.2f, 1f); // Darker gray

        private ReorderableList reorderableList;

        // Used to change the rect background
        private Texture2D texture;

        private void SetupReorderableList()
        {
            reorderableList = new ReorderableList(serializedObject, drops, true, true, true, true)
            {
                drawHeaderCallback = DrawHeaderCallback,
                drawElementCallback = DrawElementCallback,
                elementHeightCallback = ElementHeightCallback,
                drawElementBackgroundCallback = DrawElementBackgroundCallback,
                onAddCallback = OnAddCallback
            };

            texture = new Texture2D(1, 1);
        }

        private void OnAddCallback (ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);

            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            // This bypass the checks on drop properties by changing directly the fields
            InitDropElement(element);
        }

        private static void DrawHeaderCallback (Rect rect)
            => EditorGUI.LabelField(rect, "Drops");

        private void DrawElementCallback (Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, element, GUIContent.none);
        }

        private float ElementHeightCallback (int index)
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            return EditorGUI.GetPropertyHeight(element);
        }

        private void DrawElementBackgroundCallback (Rect rect, int index, bool isActive, bool isFocused)
        {
            Color selectedColor;
            if (isActive)
                selectedColor = isActiveColor;
            else
                selectedColor = index % 2 == 0 ? isEvenColor : isOddColor;

            ChangeTextureColorTo(selectedColor);
            GUI.DrawTexture(rect, texture);
        }

        private void InitDropElement (SerializedProperty element)
        {
            element.FindPropertyRelative("entry").objectReferenceValue = null;
            element.FindPropertyRelative("amountRange").vector2IntValue = new Vector2Int(1, 1);
            element.FindPropertyRelative("amountLimit").vector2IntValue = new Vector2Int(0, 10);
            element.FindPropertyRelative("weight").intValue = 1;
            element.FindPropertyRelative("odds").floatValue = 0f;
            element.FindPropertyRelative("isExtension").boolValue = false;
            element.FindPropertyRelative("isGuaranteed").boolValue = false;
            element.FindPropertyRelative("isDisabled").boolValue = false;
            element.FindPropertyRelative("ownerTable").objectReferenceValue = Target;
        }

        private void ChangeTextureColorTo (Color targetColor)
        {
            texture.SetPixel(0, 0, targetColor);
            texture.Apply();
        }
    }
}
#endif