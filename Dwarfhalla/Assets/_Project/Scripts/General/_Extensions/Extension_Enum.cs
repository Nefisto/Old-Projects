using System;

public static partial class Extension
{
    public static string ToLabel (this SummonCard.AttackType attackType)
        => attackType switch
        {
            SummonCard.AttackType.SingleTarget => "Single Target",
            SummonCard.AttackType.DamageAllTargets => "All Targets",
            _ => throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null)
        };
}