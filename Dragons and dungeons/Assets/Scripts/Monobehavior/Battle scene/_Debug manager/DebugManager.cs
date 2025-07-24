#if UNITY_EDITOR
using NTools;
using Sirenix.OdinInspector;

/// <summary>
/// Class for editor only, will be used to 
/// </summary>
public class DebugManager : SingletonMonoBehaviour<DebugManager>
{
    [TabGroup("Battle")]
    [Title("Prints")]
    public bool showInitialDialog = true;
}
#endif