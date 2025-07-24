using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HoldingIcon : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image icon;

    [TitleGroup("References")]
    [SerializeField]
    private Image background;

    [TitleGroup("References")]
    [SerializeField]
    private Image buffOrDebuff;

    public IEnumerator Setup (ChargeAbility settings)
    {
        if (settings is InstantChargeAbility instantChargeAbility)
            return Setup(instantChargeAbility);

        return Setup((PersistentChargeAbility)settings);
    }

    private IEnumerator Setup (InstantChargeAbility settings)
    {
        icon.sprite = Database
            .GameIcons
            .StatusEffectIcons
            .GetIconOfKind(settings.IconKind);

        background.color = Database
            .GameConstantsSo
            .ChargeLevelToColor[settings.AbilityLevel];

        yield break;
    }

    private IEnumerator Setup (PersistentChargeAbility settings)
    {
        icon.sprite = Database
            .GameIcons
            .StatusEffectIcons
            .GetIconOfKind(settings.IconKind);

        buffOrDebuff.enabled = true;
        buffOrDebuff.sprite = Database
            .GameIcons
            .BuffOrDebuffIcons
            .GetIconOfKind(StatusEffectData.BuffOrDebuff.Buff);

        buffOrDebuff.enabled = true;

        background.color = Database
            .GameConstantsSo
            .ChargeLevelToColor[settings.AbilityLevel];

        yield break;
    }
}