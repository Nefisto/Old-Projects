using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[Serializable]
public enum Sounds
{
    SoundA,
    SoundB,
    SoundC
}

[Serializable]
public class Sound
{
    public string name;

    [SerializeField]
    public Sounds nameEnum;
}

public interface OneInterface
{
    public void Show();
}

public partial class TestA : MonoBehaviour
{
    public Transform actionFolder;
    public GameObject panelPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var instance = Instantiate(panelPrefab, actionFolder, true).GetComponent<RectTransform>();
            instance.anchoredPosition3D = Vector3.zero;
        }
    }

    [Button]
    public void Show()
    {
        _ = InfoCardPanel.Instance;
        // InfoCardPanel.Instance.ShowInfoCard(new InfoCardContext()
        // {
        //     Name = "Danone",
        //     Description = "Muito gosto mesmo"
        // });
    }

    public void T2()
    {
        Debug.Log($"HI");
    }
}