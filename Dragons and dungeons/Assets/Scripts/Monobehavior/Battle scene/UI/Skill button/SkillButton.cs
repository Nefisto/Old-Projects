using NTools;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Its the button of skill that appear on UI
/// </summary>
[RequireComponent(typeof(Button))]
public class SkillButton : LazyBehavior
{
    public void Setup (Sprite icon)
    {
        image.sprite = icon;
    }
}