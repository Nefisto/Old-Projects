#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

public class RefreshAllFontMixer : Editor
{
    [MenuItem("Tools/Untitled RPG/Refresh all font mixer")]
    private static void SelectGameObjects()
    {
        var currentScene = SceneManager.GetActiveScene();
        var allMixers = currentScene
            .GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<FontMixer>())
            .Where(fm => fm is not null);

        var fm = FontMixerManager.Instance;
        foreach (var fontMixer in allMixers)
        {
            fontMixer.RefreshFont(fm);
            EditorUtility.SetDirty(fm);
        }
    }
}
#endif