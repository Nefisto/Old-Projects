using System;
using System.Collections;

public partial class SessionManager
{
    public static event Func<IEnumerator> OnLoadThings;

    public static IEnumerator LoadSession()
    {
        foreach (var method in OnLoadThings?.GetInvocationList()!)
        {
            yield return method?.DynamicInvoke();
        }
    }
}