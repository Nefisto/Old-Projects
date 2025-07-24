public static class GameEditorSettings
{
    public static class MenuNames
    {
        public const string FileNameIdleState = "Idle";
        public const string FileNameWanderState = "Wander";
        public const string FileNameFleeState = "Flee";

        private const string MenuNameBase = "I See E'm/";
        private const string MenuNameEnemyStates = "Enemy states/";
        public const string MenuNameIdleState = MenuNameBase + MenuNameEnemyStates + FileNameIdleState;
        public const string MenuNameWanderState = MenuNameBase + MenuNameEnemyStates + FileNameWanderState;
        public const string MenuNameFleeState = MenuNameBase + MenuNameEnemyStates + FileNameFleeState;
    }
}