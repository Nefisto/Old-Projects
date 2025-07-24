using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public class TraitBuildHUD : SerializedMonoBehaviour, IMenu
{
    [Space]
    [TitleGroup("References")]
    [SerializeField]
    private CharacterBuildRemainingPointsHUD remainingPointsHUD;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeBuildSectorHUD strengthSectorHUD;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeBuildSectorHUD vitalitySectorHUD;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeBuildSectorHUD dexteritySectorHUD;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeBuildSectorHUD intelligenceSectorHUD;

    [DisableInEditorMode]
    [Button]
    public IEnumerator Setup (TraitChart traitChartToLoad = null)
    {
        var selectedChart = traitChartToLoad
                            ?? ServiceLocator.SessionManager.PlayableCharacterData.TraitChart;

        Assert.IsNotNull(selectedChart);

        remainingPointsHUD.Setup(selectedChart);

        strengthSectorHUD.Setup(selectedChart, selectedChart.StrengthSector);
        vitalitySectorHUD.Setup(selectedChart, selectedChart.VitalitySector);
        dexteritySectorHUD.Setup(selectedChart, selectedChart.DexteritySector);
        intelligenceSectorHUD.Setup(selectedChart, selectedChart.IntelligenceSector);
        yield break;
    }
}