#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Observable<>))]
public class ObservablePropertyDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var valueProperty = property.FindPropertyRelative("value");
        EditorGUI.PropertyField(position, valueProperty, new GUIContent(property.displayName));

        EditorGUI.EndProperty();
    }
}
#endif