using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIconHUD : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image icon;

    [TitleGroup("References")]
    [SerializeField]
    private Image background;

    public IEnumerator Setup (Settings settings)
    {
        icon.sprite = settings.icon;
        background.color = settings.color;
        yield break;
    }

    public class Settings
    {
        public Color32 color;
        public Sprite icon;
    }
}