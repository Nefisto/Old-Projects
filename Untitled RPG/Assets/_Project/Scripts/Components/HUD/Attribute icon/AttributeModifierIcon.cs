using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class AttributeModifierIcon : SerializedMonoBehaviour
{
    [TitleGroup("Settings")]
    [OdinSerialize]
    private Dictionary<int, Transform> modifierLevelToFolder = new();

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public void UpdateCurrentLevel (int newLevel)
    {
        DisableAllLevels();

        modifierLevelToFolder[newLevel]
            .gameObject
            .SetActive(true);
    }

    public void DisableAllLevels()
    {
        foreach (var (_, folder) in modifierLevelToFolder)
            folder.gameObject.SetActive(false);
    }
}