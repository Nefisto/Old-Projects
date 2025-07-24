public class CastSkillCombatLog : CombatLogEntry
{
    public sealed override string MessageTemplate => "<color={0}>{1}</color> used skill <color={2}>{3}</color>";

    public CastSkillCombatLog(BattleActor actor, Skill skill)
    {
        var actorColor = CombatLogUtilities.GetBattleActorRichTextColor(actor);
        var skillColor = CombatLogUtilities.GetSkillRichColor();

        Message = string.Format(MessageTemplate, actorColor, actor.name, skillColor, skill.name);
    }
}