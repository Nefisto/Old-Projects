public class EndTurnCombatLog : CombatLogEntry
{
    public sealed override string MessageTemplate => "<color={0}>{1}</color> ended their turn";

    public EndTurnCombatLog(BattleActor actor)
    {
        var color = CombatLogUtilities.GetBattleActorRichTextColor(actor);
        Message = string.Format(MessageTemplate, color, actor.name);
    }
}