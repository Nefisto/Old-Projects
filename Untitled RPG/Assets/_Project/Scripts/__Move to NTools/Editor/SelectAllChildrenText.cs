#if UNITY_EDITOR
using System.Linq;
using TMPro;
using UnityEditor;

public class SelectAllChildrenText : Editor
{
    [MenuItem("Tools/Untitled RPG/Select all TMP_Text under")]
    private static void SelectGameObjects()
    {
        Selection.objects = Selection
            .activeGameObject
            .GetComponentsInChildren<TMP_Text>(true)
            .Where(t => t.GetComponent<FontMixer>() == null)
            .Select(t => t.gameObject)
            .ToArray();
    }
}
#endif