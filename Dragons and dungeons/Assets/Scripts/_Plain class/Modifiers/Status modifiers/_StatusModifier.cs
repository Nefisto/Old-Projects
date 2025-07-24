public abstract class StatusModifier : Modifier
{
    protected readonly int amountToModify; // If this value change on runtime we end with a permanent modifier 
    protected int initialTurnsDuration;

    public int TurnsLeft { get; private set; }

    public StatusModifier(int amountToModify, int turnsDuration)
    {
        this.amountToModify = amountToModify;
        initialTurnsDuration = turnsDuration;

        TurnsLeft = initialTurnsDuration;
    }

    public override void StartTurnEffect (ModifierContext modifierContext)
    {
        base.StartTurnEffect(modifierContext);

        TurnsLeft--;
        if (TurnsLeft <= 0)
        {
            Remove(modifierContext);
        }
    }

    public override void RefreshModifier()
    {
        base.RefreshModifier();

        TurnsLeft = initialTurnsDuration;
    }
}