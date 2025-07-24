using NTools;

public static class Utilities
{
    public static void CreateLog (string title, string description, float timeInScreen = 3f)
    {
        var ctx = new LogContext()
        {
            Title = title,
            Description = description,
            TimeInScreen = timeInScreen
        };
        EventHandler.RaiseEvent(GameEvents.CREATE_LOG, ctx);
    }
}