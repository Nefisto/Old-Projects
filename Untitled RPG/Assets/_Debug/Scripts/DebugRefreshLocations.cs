using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class DebugRefreshLocations : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Dropdown dropdown;

    [TitleGroup("References")]
    [SerializeField]
    private Button specificLocationButton;

    private List<(string, LocationModifier)> nameToModifier = new();

    private void Awake()
    {
        dropdown.enabled = false;
        GameEvents.onFinishedLoadingData += FillDropdown;

        specificLocationButton.onClick.RemoveAllListeners();
        specificLocationButton.onClick.AddListener(() =>
        {
            var selectedLocation = dropdown.options[dropdown.value].text;
            var modifier = nameToModifier
                .First(t => t.Item1 == selectedLocation)
                .Item2;
            Location.Test_SetThisModifierToEveryLocation(modifier);
        });
    }

    private void FillDropdown()
    {
        dropdown.enabled = true;

        nameToModifier = new();
        nameToModifier.AddRange(Database
            .LocationsModifiers
            .Data
            .Select(d => (d.name, d)));

        dropdown.options.Clear();
        dropdown.AddOptions(nameToModifier
            .Select(t => t.Item1)
            .ToList());
        dropdown.RefreshShownValue();
    }

    public void RefreshLocations()
    {
        Location.Test_GetANewModifierForAllLocations();
    }
}
#endif