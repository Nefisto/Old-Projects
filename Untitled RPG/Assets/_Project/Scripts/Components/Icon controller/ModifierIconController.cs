using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class ModifierIconController : IconController
{
    [TitleGroup("Settings")]
    [SerializeField]
    private StatusEffectIcon iconPrefab;

    public override IEnumerator CreateIcon (StatusEffectData instance)
    {
        var icon = Database
            .GameIcons
            .StatusEffectIcons
            .GetIconOfKind(instance.Kind);

        var iconInstance = Instantiate(iconPrefab, transform, false);
        var settings = new StatusEffectIcon.Settings { icon = icon };

        if (instance is IStackableStatusEffect stackableStatusEffect)
        {
            settings.stackAmount = stackableStatusEffect.StackAmount;
            stackableStatusEffect.OnUpdatedStackAmount += () =>
                iconInstance.RefreshStackAmount(stackableStatusEffect.StackAmount);
        }

        iconInstance.Setup(settings);

        instance.OnStatusEffectTick += remainingPercentage => iconInstance.GradientBar.UpdateBar(remainingPercentage);
        instance.OnStatusEffectEnd += () => Destroy(iconInstance.gameObject);
        yield return null;
    }
}