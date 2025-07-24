using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class SectorBonusHUD : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private TMP_Text completedPointsLabel;

    [TitleGroup("Settings")]
    [SerializeField]
    private AttributeLevelBarView mainAttributeLevel;

    [DisableInEditorMode]
    [Button]
    public void Setup (TraitSector attributeSector)
    {
        attributeSector.OnUpdatedAccumulatedPoints += ()
            => RefreshSectorFields(attributeSector.CompletedPoints, attributeSector.PartialPoints);

        RefreshSectorFields(attributeSector.CompletedPoints, attributeSector.PartialPoints);
    }

    private void RefreshSectorFields (int completedPoints, int partialPoints)
    {
        completedPointsLabel.text = $"{completedPoints}";
        mainAttributeLevel.Setup(partialPoints);
    }
}