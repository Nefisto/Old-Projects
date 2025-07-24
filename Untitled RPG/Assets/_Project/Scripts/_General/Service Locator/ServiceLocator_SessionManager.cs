using UnityEngine;

public static partial class ServiceLocator
{
    private static SessionManager sessionManager;

    public static SessionManager SessionManager
    {
        get
        {
            if (sessionManager is not null)
                return sessionManager;

            sessionManager = Object.FindObjectOfType<SessionManager>()
                             ?? new GameObject().AddComponent<SessionManager>();

            sessionManager.name = "[Service] SessionManager";
            return sessionManager;
        }
    }
}