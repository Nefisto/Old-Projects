using Sirenix.OdinInspector;
using UnityEngine;

public class EncounterRateBar : GradientBar
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Color32 noRiskColor;

    [TitleGroup("References")]
    [SerializeField]
    private EncounterRateSystem ers;

    protected override void UpdateColor (float percentage)
    {
        var minimumValue = ers.MinimumValue;
        if (percentage < minimumValue)
        {
            PaintBar(noRiskColor);
            return;
        }

        var actualPercentage = (percentage - minimumValue) / (1 - minimumValue);
        base.UpdateColor(actualPercentage);
    }
}