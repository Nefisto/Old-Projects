using System.IO;
using UnityEngine;

public static partial class Helper
{
    public static void SaveTemplate (string path, Template template)
    {
        var data = JsonUtility.ToJson(template, true);
        File.WriteAllText(path, data);
    }

    public static Template LoadTemplate (string path)
    {
        if (!File.Exists(path))
            return new Template();

        var data = File.ReadAllText(path);
        var loadedTemplate = JsonUtility.FromJson<Template>(data);
        loadedTemplate.LoadReferences();

        return loadedTemplate;
    }
}