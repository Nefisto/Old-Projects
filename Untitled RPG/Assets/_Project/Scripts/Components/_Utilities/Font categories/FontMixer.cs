using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
    
[DisallowMultipleComponent]
public class FontMixer : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private FontMixerGroup fontMixerGroup;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text label;

    private void OnValidate() => label ??= GetComponent<TMP_Text>();

    public void RefreshFont(FontMixerManager fontMixer)
    {
        label.font = fontMixer.GetFontForGroup(fontMixerGroup);
    }

    public enum FontMixerGroup
    {
        Menu,
        Battle
    }
}