using System;
using System.Collections.Generic;

/// <summary>
/// This guy will be responsable to tell to skill which are the important attributes to showcase first on cases like HUD
/// </summary>
public static class MainAttributesOrderProvider
{
    public static IEnumerable<SkillAttribute> GetCorrectOrder (MainAttributeOrder order)
        => order switch
        {
            MainAttributeOrder.TypicalDamage => CommonDamageSkill(),
            MainAttributeOrder.JustApplyStatus => JustStatusApply(),
            _ => throw new ArgumentOutOfRangeException(nameof(order), order, null)
        };

    private static IEnumerable<SkillAttribute> CommonDamageSkill()
    {
        yield return SkillAttribute.ManaCost;
        yield return SkillAttribute.Cooldown;
    }

    private static IEnumerable<SkillAttribute> JustStatusApply()
    {
        yield return SkillAttribute.ManaCost;
        yield return SkillAttribute.ChanceToHit;
        yield return SkillAttribute.Cooldown;
    }
}