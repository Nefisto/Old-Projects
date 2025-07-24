using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FontMixerManager : SingletonScriptableObject<FontMixerManager>
{
    [TitleGroup("Settings")]
    [SerializeField]
    private NDictionary<FontMixer.FontMixerGroup, TMP_FontAsset> groupToFont;

    public TMP_FontAsset GetFontForGroup (FontMixer.FontMixerGroup group) => groupToFont[group];
}