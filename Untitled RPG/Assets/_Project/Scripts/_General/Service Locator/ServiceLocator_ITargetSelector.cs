public static partial class ServiceLocator
{
    private static ITargetSelector targetSelector = new NullSelector();

    public static ITargetSelector TargetSelector
    {
        get => targetSelector ??= new NullSelector();
        set => targetSelector = value;
    }
}