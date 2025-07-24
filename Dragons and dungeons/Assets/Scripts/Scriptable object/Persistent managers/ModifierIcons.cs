using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ModifierIcons
{
    [HorizontalGroup("split", .7f)]
    [HideLabel]
    public ModifierKind kind;

    [HorizontalGroup("split")]
    [HideLabel]
    [PreviewField]
    public Sprite icon;
}