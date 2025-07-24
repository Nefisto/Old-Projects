public class HealCombatLog : CombatLogEntry
{
    public sealed override string MessageTemplate => "<color={0}>{1}</color> get healed for {2}. {3} increased by WIS power";

    public HealCombatLog(BattleActor actor, int healedAmount, int increasedAmount)
    {
        var actorColor = CombatLogUtilities.GetBattleActorRichTextColor(actor);

        Message = string.Format(MessageTemplate, actorColor, actor.name, healedAmount, increasedAmount);
    }
}