using System;

public static partial class Helper
{
    public static FloatTextKind FromDamageKindToFloatTextKind (DamageKind kind)
        => kind switch
        {
            DamageKind.Normal => FloatTextKind.Normal,
            DamageKind.Poison => FloatTextKind.Poison,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
}