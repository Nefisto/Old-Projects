using System;

public static partial class Extensions
{
    public static EnemyStates ToEnumStates(this EnemyState state)
        => state switch
        {
            FleeState _ => EnemyStates.Flee,
            PauseState _ => EnemyStates.Pause,
            WanderState _ => EnemyStates.Wander,
            _ => EnemyStates.Idle
        };
}