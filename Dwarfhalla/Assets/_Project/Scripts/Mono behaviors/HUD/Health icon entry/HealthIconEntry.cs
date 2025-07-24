using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HealthIconEntry : MonoBehaviour
{
    public enum HealthIconType
    {
        Fixed,
        Extra
    }

    [TitleGroup("Settings")]
    [SerializeField]
    private Color fixedFullColor = Color.green;

    [TitleGroup("Settings")]
    [SerializeField]
    private Color fixedEmptyColor = Color.gray;

    [TitleGroup("Settings")]
    [SerializeField]
    private Color extraFullColor = Color.cyan;

    [TitleGroup("References")]
    [SerializeField]
    private Image icon;

    private HealthIconType healthType;
    public Action OnDeplete;

    public void Setup (HealthIconType healthIconEntry)
    {
        healthType = healthIconEntry;
        Fill();
    }

    public void Fill() => icon.color = healthType == HealthIconType.Fixed ? fixedFullColor : extraFullColor;

    public void Deplete()
    {
        OnDeplete?.Invoke();

        if (healthType == HealthIconType.Fixed)
            icon.color = fixedEmptyColor;
    }
}