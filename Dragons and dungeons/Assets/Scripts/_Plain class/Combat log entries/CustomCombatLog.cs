public class CustomCombatLog : CombatLogEntry
{
    public override string MessageTemplate => "";

    public CustomCombatLog(string customMessage)
    {
        Message = customMessage;
    }
}