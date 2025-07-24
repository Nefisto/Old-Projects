using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Poison", menuName = EditorConstants.MODIFIER_PATH + "Poison")]
public class Poison : Modifier
{
    [TitleGroup("Settings")]
    [SerializeField]
    private NDictionary<int, Sprite> levelToIcon = new();

    private int currentLevel;

    public override ModifierKind Kind => ModifierKind.Poison;

    public override Sprite GetIcon() => levelToIcon[currentLevel];

    public override IEnumerator Apply (ModifierSettings settings)
    {
        cachedSettings = settings;

        currentLevel = 1;

        settings.caster.OnTurnStart += PoisonHandle;
        yield break;
    }

    public override IEnumerator Reapply (ModifierSettings settings)
    {
        currentLevel = Mathf.Max(currentLevel, 3);
        OnRefresh?.Invoke();
        yield break;
    }

    public override IEnumerator Remove()
    {
        cachedSettings.caster.OnTurnStart -= PoisonHandle;

        yield return base.Remove();
    }

    private IEnumerator PoisonHandle (object _)
    {
        yield return cachedSettings.target.TakeDamage(new BlockData.ApplyDamageSettings
        {
            damage = 1,
            damageColor = Color.green
        });

        if (--currentLevel <= 0)
            yield return Remove();
    }
}