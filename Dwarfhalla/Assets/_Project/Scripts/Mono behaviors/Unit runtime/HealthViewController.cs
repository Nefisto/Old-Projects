using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthViewController : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Transform healthIconFolder;

    [TitleGroup("References")]
    [SerializeField]
    private HealthIconEntry healthIconEntry;

    private List<HealthIconEntry> cachedIcons = new();

    private UnitData unitData;

    public void Setup (UnitData unitData)
    {
        this.unitData = unitData;
        this.unitData.OnUpdateHealth += UpdateHealth;

        healthIconFolder.DestroyChildren();
        for (var i = 0; i < unitData.MaxHealth; i++)
        {
            var iconInstance = Instantiate(healthIconEntry, healthIconFolder, false);
            iconInstance.Setup(HealthIconEntry.HealthIconType.Fixed);

            cachedIcons.Add(iconInstance);
        }
    }

    public void UpdateHealth()
    {
        var counter = unitData.CurrentHealth;
        foreach (var healthIcon in cachedIcons.ToList())
        {
            if (counter > 0)
                healthIcon.Fill();
            else
                healthIcon.Deplete();

            counter--;
        }

        while (counter-- > 0)
        {
            var iconInstance = Instantiate(healthIconEntry, healthIconFolder, false);
            iconInstance.Setup(HealthIconEntry.HealthIconType.Extra);
            iconInstance.OnDeplete += () =>
            {
                cachedIcons.Remove(iconInstance);
                Destroy(iconInstance.gameObject);
            };

            cachedIcons.Add(iconInstance);
        }
    }
}