public static class CombatLogUtilities
{
    private const string AllyNameColor = "green";
    private const string EnemyNameColor = "red";

    private const string SkillNameColor = "yellow";
    private const string ModifierNameColor = "blue";
    
    public static string GetBattleActorRichTextColor (BattleActor actor)
        => actor is FriendlyBattleActor ? AllyNameColor : EnemyNameColor;

    public static string GetSkillRichColor ()
        => SkillNameColor;

    public static string GetModifierRichColor()
        => ModifierNameColor;
}