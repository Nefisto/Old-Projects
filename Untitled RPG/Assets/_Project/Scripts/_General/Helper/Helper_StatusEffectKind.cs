using System;

public static partial class Helper
{
    public static FloatTextKind StatusEffectKindToFloatTextKind (StatusEffectKind kind)
        => kind switch
        {
            StatusEffectKind.None => FloatTextKind.Normal,
            StatusEffectKind.Poison => FloatTextKind.Poison,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
}