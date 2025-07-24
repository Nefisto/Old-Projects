using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield modifier", menuName = EditorConstants.MODIFIER_PATH + "Shield modifier")]
public class ShieldModifier : Modifier
{
    public override ModifierKind Kind => ModifierKind.Shield;

    public override IEnumerator Apply (ModifierSettings settings)
    {
        cachedSettings = settings;

        settings.target.OnTakingDamage += DamageHandle;
        yield break;
    }

    public override IEnumerator Remove()
    {
        cachedSettings.target.OnTakingDamage -= DamageHandle;

        yield return base.Remove();
    }

    private IEnumerator DamageHandle (object arg)
    {
        var ctx = (BlockData.ApplyDamageSettings)arg;

        ctx.damage = 0;
        yield return Remove();
    }
}