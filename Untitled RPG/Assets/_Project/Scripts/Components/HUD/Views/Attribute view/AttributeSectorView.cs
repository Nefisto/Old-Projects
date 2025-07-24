using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class AttributeSectorView : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text sectorLevelLabel;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeLevelBarView sectorLevelBar;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeView attributeA, attributeB, attributeC;

    public void Setup (TraitSector attributeChart)
    {
        sectorLevelLabel.text = $"{attributeChart.CompletedPoints}";
        sectorLevelBar.Setup(attributeChart.PartialPoints);

        var viewToAttribute = attributeChart
            .Zip(new List<AttributeView> { attributeA, attributeB, attributeC },
                (attribute, view) => (view, attribute));
        foreach (var (view, attribute) in viewToAttribute)
            view.Setup(attribute);
    }
}