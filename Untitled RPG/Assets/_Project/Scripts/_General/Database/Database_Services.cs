using System.Collections;

public static partial class Database
{
    public static DataLoader<MenuStack> ServiceMenuStack { get; } = new("MenuStack", false);

    private static IEnumerator LoadServices()
    {
        yield return ServiceMenuStack.LoadData();
    }

    private static void UnloadServices()
    {
        ServiceMenuStack.UnloadData();
    }
}