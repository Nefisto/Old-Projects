using UnityEngine;

public static partial class ServiceLocator
{
    public static GameContext GameContext { get; set; }
    public static MouseController MouseController { get; set; }

    /// <summary>
    ///     Custom image used for multiple purposes, mostly feedbacks
    /// </summary>
    public static BackgroundImage BackgroundImage { get; set; }

    /// <summary>
    /// Used until adresables get fixed 
    /// </summary>
    public static Database Database { get; set; }

    public static GameplayMessage GameplayMessage { get; set; }
    public static ScreenFading ScreenFading { get; set; }

    public static ITooltip Tooltip { get; set; } = new NullTooltip();

    public static ICoinPooler CoinPooler { get; set; } = new NullCoinPooler();

    // SHAME
    // Empty game object to notify some events to FMOD studio
    public static GameObject GlobalNotifyObject { get; set; }
}