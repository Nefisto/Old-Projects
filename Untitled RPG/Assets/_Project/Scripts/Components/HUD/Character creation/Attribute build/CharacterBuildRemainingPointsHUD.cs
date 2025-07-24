using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CharacterBuildRemainingPointsHUD : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private string messageTemplate = "Remaining points {0}";

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text label;

    [DisableInEditorMode]
    [Button]
    public void Setup (TraitChart traitChart)
    {
        traitChart.OnUpdateSpendPoints += ()
            => label.text = string.Format(messageTemplate, GetRemainingPoints(traitChart));

        label.text = string.Format(messageTemplate, GetRemainingPoints(traitChart));
    }

    private static int GetRemainingPoints (TraitChart traitChart)
        => Database.AccountData.PointsToDistribute - traitChart.SpentPoints;
}