using System;
using Sirenix.OdinInspector;

[Serializable]
public class Potential
{
    public const string NicefyTemplate = "<color={0}>{1}</color>";

    [HorizontalGroup("Line A")]
    [LabelText("STR:")]
    [MinValue(0)]
    [MaxValue(9)]
    public int strength;

    [HorizontalGroup("Line A")]
    [LabelText("VIT:")]
    [MinValue(0)]
    [MaxValue(9)]
    public int vitality;

    [HorizontalGroup("Line B")]
    [LabelText("DEX:")]
    [MinValue(0)]
    [MaxValue(9)]
    public int dexterity;

    [HorizontalGroup("Line B")]
    [LabelText("INT:")]
    [MinValue(0)]
    [MaxValue(9)]
    public int intelligence;

    public string NicefiedString
        => $"<color={GameConstants.STRENGTH_HEX_COLOR}>{strength}</color>"
           + $"<color={GameConstants.VITALITY_HEX_COLOR}>{vitality}</color>"
           + $"<color={GameConstants.DEXTERITY_HEX_COLOR}>{dexterity}</color>"
           + $"<color={GameConstants.INTELLIGENCE_HEX_COLOR}>{intelligence}</color>";

    public string NicefiedStringWithMain (MainTraitKind mainTraitKind)
    {
        var strengthWord = string.Format(NicefyTemplate, GameConstants.STRENGTH_HEX_COLOR, strength);
        var vitalityWord = string.Format(NicefyTemplate, GameConstants.VITALITY_HEX_COLOR, vitality);
        var dexterityWord = string.Format(NicefyTemplate, GameConstants.DEXTERITY_HEX_COLOR, dexterity);
        var intelligenceWord = string.Format(NicefyTemplate, GameConstants.INTELLIGENCE_HEX_COLOR, intelligence);

        switch (mainTraitKind)
        {
            case MainTraitKind.Strength:
                strengthWord = AddSize(strengthWord);
                break;

            case MainTraitKind.Vitality:
                vitalityWord = AddSize(vitalityWord);
                break;

            case MainTraitKind.Intelligence:
                intelligenceWord = AddSize(intelligenceWord);
                break;

            case MainTraitKind.Dexterity:
                dexterityWord = AddSize(dexterityWord);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(mainTraitKind), mainTraitKind, null);
        }

        return $"{strengthWord}{vitalityWord}{dexterityWord}{intelligenceWord}";

        string AddSize (string word) => $"<b><size=21>{word}</size></b>";
    }
}