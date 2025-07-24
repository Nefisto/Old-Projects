public class TakeDamageCombatLog : CombatLogEntry
{
    public sealed override string MessageTemplate => "<color={0}>{1}</color> took {2} damage. Resisted: {3}";

    public TakeDamageCombatLog(BattleActor actor, int tookDamage, int resistedDamage)
    {
        var color = CombatLogUtilities.GetBattleActorRichTextColor(actor);

        Message = string.Format(MessageTemplate, color, actor.name, tookDamage, resistedDamage);
    }
}