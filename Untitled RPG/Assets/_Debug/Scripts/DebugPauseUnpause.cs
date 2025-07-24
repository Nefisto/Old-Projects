using UnityEngine;

public class DebugPauseUnpause : MonoBehaviour
{
    public void Pause() => GameEvents.OnPause?.Invoke();

    public void Unpause() => GameEvents.OnUnpause?.Invoke();
}