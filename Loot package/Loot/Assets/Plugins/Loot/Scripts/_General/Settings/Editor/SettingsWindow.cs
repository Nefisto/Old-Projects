#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Loot.Editor
{
    public class SettingsWindow : EditorWindow
    {
        private void Awake()
        {
            if (!TryReadFromFile("Loot settings.json", out var json))
                return;

            var instance = JsonUtility.FromJson<LootSettings>(json);
            LootSettings.SetInstance(instance);
        }

        private void OnEnable()
            => Undo.undoRedoPerformed += Repaint;

        private void OnDisable()
            => Undo.undoRedoPerformed -= Repaint;

        private void OnDestroy()
        {
            var json = JsonUtility.ToJson(LootSettings.Instance, true);
            WriteToFile("Loot settings.json", json);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Settings:", EditorStyles.boldLabel);

            LootSettings.MaxDepthLayers = EditorGUILayout.IntField("Max depth:", LootSettings.MaxDepthLayers);
            LootSettings.MaxDropsPerCall = EditorGUILayout.IntField("Max drops per request:", LootSettings.MaxDropsPerCall);
            EditorGUILayout.HelpBox("Enabling this will make your changes to be persistent between editor sessions", MessageType.Warning);
            LootSettings.EnableWorkOnOriginal = EditorGUILayout.Toggle("Allow work on original:", LootSettings.EnableWorkOnOriginal);
        }

        [MenuItem(EditorConstants.SettingWindow, priority = -50)]
        private static void ShowWindow()
        {
            var window = GetWindow<SettingsWindow>();
            window.titleContent = new GUIContent("Drops settings");
            window.Show();
        }

        public void WriteToFile (string fileName, string json)
        {
            var path = GetFilePath(fileName);
            var fileStream = new FileStream(path, FileMode.Create);

            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }

        public bool TryReadFromFile (string fileName, out string json)
        {
            var path = GetFilePath(fileName);
            if (!File.Exists(path))
            {
                json = "";
                return false;
            }

            using (var reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
                return true;
            }
        }

        private string GetFilePath (string fileName)
            => Application.persistentDataPath + "/" + fileName;
    }
}
#endif