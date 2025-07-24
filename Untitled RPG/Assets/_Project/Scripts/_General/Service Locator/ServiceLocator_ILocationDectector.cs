public static partial class ServiceLocator
{
    private static ILocationDetector locationDetector = new NullLocationDetector();

    public static ILocationDetector LocationDetector
    {
        get => locationDetector ??= new NullLocationDetector();
        set => locationDetector = value;
    }
}