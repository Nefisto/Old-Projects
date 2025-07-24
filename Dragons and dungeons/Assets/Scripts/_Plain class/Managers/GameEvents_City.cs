using System;

public static partial class GameEvents
{
    public static class City
    {
        public static event Action OnStart;

        public static void RaiseStart()
            => OnStart?.Invoke();
    }
}