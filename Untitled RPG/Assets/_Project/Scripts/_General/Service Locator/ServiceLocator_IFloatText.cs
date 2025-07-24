public static partial class ServiceLocator
{
    private static IFloatText floatText = new NullFloatText();

    public static IFloatText FloatText
    {
        get => floatText ??= new NullFloatText();
        set => floatText = value;
    }
}