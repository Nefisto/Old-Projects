public class ManaGainedLog : CombatLogEntry
{
    public sealed override string MessageTemplate => "<color={0}>{1}</color> collected: <color=red>{2}x Red</color> - <color=green>{3}x Green</color> - <color=blue>{4}x Blue</color>";

    public ManaGainedLog(BattleActor actor, ManaPool manaPool)
    {
        var actorColor = CombatLogUtilities.GetBattleActorRichTextColor(actor);

        Message = string.Format(MessageTemplate, actorColor, actor.name, manaPool.red, manaPool.green, manaPool.blue);
    }
}