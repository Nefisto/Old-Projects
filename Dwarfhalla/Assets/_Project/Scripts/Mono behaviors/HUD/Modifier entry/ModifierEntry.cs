using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ModifierEntry : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image icon;

    public void Setup (Modifier modifier)
    {
        icon.sprite = modifier.GetIcon();
    }
}