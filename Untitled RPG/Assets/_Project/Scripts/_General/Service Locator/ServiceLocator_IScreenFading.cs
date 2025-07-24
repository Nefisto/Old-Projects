using UnityEngine;
using UnityEngine.Assertions;

public static partial class ServiceLocator
{
    private static IScreenFading screenFading;

    public static IScreenFading ScreenFading
    {
        get
        {
            if (screenFading != null)
                return screenFading;
            
            return (IScreenFading)Object.FindObjectOfType<ScreenFading>() ?? new NullScreenFading();
        }
        set => screenFading = value;
    }
}