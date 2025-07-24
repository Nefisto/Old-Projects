using System;

public static partial class GameEvents
{
    public static class Battle
    {
        public static event Action<BattleEncounterContext> OnSetupBattle;
        public static event Action OnBattleStart;
        
        public static event Action<CombatLogEntry> OnLogBattleAction;
        
        public static void RaiseSetupBattle (BattleEncounterContext ctx)
            => OnSetupBattle?.Invoke(ctx);

        public static void RaiseCombatLogAction(CombatLogEntry combatLogEntry)
            => OnLogBattleAction?.Invoke(combatLogEntry);

        public static void RaiseBattleStart()
            => OnBattleStart?.Invoke();
    }
}