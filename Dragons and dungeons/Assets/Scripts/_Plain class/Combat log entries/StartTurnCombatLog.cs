public class StartTurnCombatLog : CombatLogEntry
{
    public sealed override string MessageTemplate => "<color={0}>{1}</color> started turn";

    public StartTurnCombatLog(BattleActor actor)
    {
        var color = CombatLogUtilities.GetBattleActorRichTextColor(actor);
        Message = string.Format(MessageTemplate, color, actor.name);
    }
}