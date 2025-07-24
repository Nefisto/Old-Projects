#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectGameObjectsWithMissingScripts : Editor
{
    [MenuItem("Tools/NTools/Select GameObjects With Missing Scripts")]
    private static void SelectGameObjects()
    {
        var currentScene = SceneManager.GetActiveScene();
        var allTransforms = currentScene
            .GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<Transform>());
        
        Object[] objectsWithDeadLinks =
            (from transform in allTransforms
                let hasAnyMissingScript = transform
                    .GetComponents<Component>()
                    .Any(c => c == null)
                where hasAnyMissingScript
                select transform.gameObject)
            .ToArray();

        if (objectsWithDeadLinks.Any())
            Selection.objects = objectsWithDeadLinks;
        else
            Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts");
    }
}
#endif