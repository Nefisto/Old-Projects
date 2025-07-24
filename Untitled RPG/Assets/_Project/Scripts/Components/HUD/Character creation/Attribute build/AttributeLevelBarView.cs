using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public class AttributeLevelBarView : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private List<GameObject> levelImages;

    [DisableInEditorMode]
    [Button]
    public void Setup (int partialLevel)
    {
        Assert.IsTrue(partialLevel is >= 0 and < 4, $"Partial level is {partialLevel}");

        DisableAllLevelImages();

        EnableImageOfLevel(partialLevel);
    }

    private void EnableImageOfLevel (int level)
        => levelImages[level].SetActive(true);

    private void DisableAllLevelImages()
        => levelImages.ForEach(go => go.SetActive(false));
}