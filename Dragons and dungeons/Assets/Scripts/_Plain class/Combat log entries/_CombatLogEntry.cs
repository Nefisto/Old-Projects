public abstract class CombatLogEntry
{
    public abstract string MessageTemplate { get; }
    
    public string Message { get; protected set; }

    public override string ToString()
        => Message;
}