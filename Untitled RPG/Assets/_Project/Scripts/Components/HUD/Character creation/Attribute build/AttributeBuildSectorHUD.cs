using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class AttributeBuildSectorHUD : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private List<AttributeEntryHUD> attributeTypeToHUDEntry;

    [TitleGroup("Settings")]
    [SerializeField]
    private SectorBonusHUD sectorBonusHUD;

    public void Setup (TraitChart traitChart, TraitSector sector)
    {
        sectorBonusHUD.Setup(sector);

        foreach (var gameAttribute in sector)
        {
            var foundTuple = attributeTypeToHUDEntry
                .First(entry => entry.AttributeType == gameAttribute.AttributeType);

            foundTuple.Setup(traitChart);
        }
    }
}