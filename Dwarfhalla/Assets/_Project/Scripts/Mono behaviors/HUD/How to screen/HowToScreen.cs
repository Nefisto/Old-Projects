using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable HeuristicUnreachableCode

public class HowToScreen : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Transform howToFolder;
    
    [TitleGroup("References")]
    [SerializeField]
    private Button closeButton;

    private bool hasClickedOnScreen;

    private void Awake()
    {
#if UNITY_EDITOR
        return;
#endif
        
#pragma warning disable CS0162 // Unreachable code detected
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            howToFolder.gameObject.SetActive(false);
            hasClickedOnScreen = true;
        });

        GameEntryPoints.OnRenderedLevel += ShowHowToHandle;
#pragma warning restore CS0162 // Unreachable code detected
    }

    private IEnumerator ShowHowToHandle (object arg)
    {
        howToFolder.gameObject.SetActive(true);
        yield return new WaitUntil(() => hasClickedOnScreen);
    }
}