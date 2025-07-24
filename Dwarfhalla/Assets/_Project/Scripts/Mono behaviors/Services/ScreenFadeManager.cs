using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ScreenFadeManager : MonoBehaviour
{
    [HideLabel]
    [SerializeField]
    private ScreenFading screenFading = new();
    
    private void Awake()
    {
        ServiceLocator.ScreenFading = screenFading;
    }
}