using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PotentialView : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text strengthPotential;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text vitalityPotential;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text dexterityPotential;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text intelligencePotential;

    public IEnumerator Setup (IPotentialProvider potentialProvider)
    {
        strengthPotential.text = $"{potentialProvider.StrengthPotential}";
        strengthPotential.color = Helper.ColorHelper.FromHex(GameConstants.STRENGTH_HEX_COLOR[1..]);

        vitalityPotential.text = $"{potentialProvider.VitalityPotential}";
        vitalityPotential.color = Helper.ColorHelper.FromHex(GameConstants.VITALITY_HEX_COLOR[1..]);

        dexterityPotential.text = $"{potentialProvider.DexterityPotential}";
        dexterityPotential.color = Helper.ColorHelper.FromHex(GameConstants.DEXTERITY_HEX_COLOR[1..]);

        intelligencePotential.text = $"{potentialProvider.IntelligencePotential}";
        intelligencePotential.color = Helper.ColorHelper.FromHex(GameConstants.INTELLIGENCE_HEX_COLOR[1..]);

        yield return null;
    }
}