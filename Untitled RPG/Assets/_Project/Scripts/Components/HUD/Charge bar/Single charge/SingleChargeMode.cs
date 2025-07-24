using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SingleChargeMode : ChargeMode
{
    [TitleGroup("References")]
    [SerializeField]
    private AbilityIconHUD abilityIconPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private Transform abilityIconFolder;

    public override IEnumerator Setup (ChargeMode.Settings settings)
    {
        abilityIconFolder.DestroyChildren();

        var correctSettings = settings as Settings;
        foreach (var chargeAbility in correctSettings.chargeAbilities)
        {
            var instance = Instantiate(abilityIconPrefab, abilityIconFolder, false);

            yield return instance.Setup(new AbilityIconHUD.Settings
            {
                icon = Database
                    .GameIcons
                    .StatusEffectIcons
                    .GetIconOfKind(chargeAbility.IconKind),
                color = Database
                    .GameConstantsSo
                    .ChargeLevelToColor[chargeAbility.AbilityLevel]
            });
        }
    }

    public new class Settings : ChargeMode.Settings
    {
        public List<ChargeAbility> chargeAbilities;
    }
}