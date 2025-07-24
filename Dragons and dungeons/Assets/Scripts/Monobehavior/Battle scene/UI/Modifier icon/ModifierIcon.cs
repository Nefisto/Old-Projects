using NTools;
using UnityEngine;

public class ModifierIcon : LazyBehavior
{
    public void Setup(Sprite icon)
    {
        image.sprite = icon;
    }
}