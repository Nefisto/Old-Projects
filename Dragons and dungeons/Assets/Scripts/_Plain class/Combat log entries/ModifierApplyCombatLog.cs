public class ModifierApplyCombatLog : CombatLogEntry
{
    public sealed override string MessageTemplate => "<color={0}>{1}</color> get <color={2}>{3}</color> effect.";

    public ModifierApplyCombatLog(BattleActor caster, string modifierName)
    {
        var actorColor = CombatLogUtilities.GetBattleActorRichTextColor(caster);
        var modifierColor = CombatLogUtilities.GetModifierRichColor();

        Message = string.Format(MessageTemplate, actorColor, caster.name, modifierColor, modifierName);
    }
}