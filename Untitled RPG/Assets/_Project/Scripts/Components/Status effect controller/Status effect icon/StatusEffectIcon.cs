using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GradientBar))]
public class StatusEffectIcon : SerializedMonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image iconImage;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text stack;

    [field: TitleGroup("References")]
    [field: SerializeField]
    public GradientBar GradientBar { get; private set; }

    public void Setup (Settings settings)
    {
        iconImage.sprite = settings.icon;
        GradientBar.UpdateBar(1f);

        stack.enabled = settings.stackAmount != 0;
        stack.text = $"{settings.stackAmount}";
    }

    public void RefreshStackAmount (int amount) => stack.text = $"{amount}";

    public class Settings
    {
        public Sprite icon;
        public int stackAmount;
    }
}