using System;

public static partial class GameEvents
{
    public static class DungeonMap
    {
        public static event Action OnFirstSetupDungeon;
        public static event Action OnStartDungeon;
        public static event Action OnSetupToResumeDungeon;
        public static event Action OnResumeDungeon;
        public static event Action OnFinishDungeon;
        
        public static void RaiseFirstSetupDungeon()
            => OnFirstSetupDungeon?.Invoke();
        
        public static void RaiseStartDungeon()
            => OnStartDungeon?.Invoke();

        public static void RaiseFinishDungeon()
            => OnFinishDungeon?.Invoke();

        public static void RaisePreToResumeDungeon()
            => OnSetupToResumeDungeon?.Invoke();

        public static void RaiseResumeDungeon()
            => OnResumeDungeon?.Invoke();
    }
}