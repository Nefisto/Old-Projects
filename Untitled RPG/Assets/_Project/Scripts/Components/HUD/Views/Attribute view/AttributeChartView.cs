using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class AttributeChartView : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private AttributeSectorView strengthSector, vitalitySector, intelligenceSector, dexteritySector;

    public IEnumerator Setup (TraitChart traitChart)
    {
        strengthSector.Setup(traitChart.StrengthSector);
        vitalitySector.Setup(traitChart.VitalitySector);
        intelligenceSector.Setup(traitChart.IntelligenceSector);
        dexteritySector.Setup(traitChart.DexteritySector);
        yield return null;
    }
}