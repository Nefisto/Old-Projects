using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class LocationLabel : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text levelLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text modifier;

    public void Refresh (LocationLabelContext ctx)
    {
        levelLabel.text = $"Level: {ctx.level:00}";
        modifier.text = ctx.locationModifier is null or EmptyModifier
            ? string.Empty
            : $"{ctx.locationModifier.NameShowOnField}({ctx.battlesRemaining})";
    }
}