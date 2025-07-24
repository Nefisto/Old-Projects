public class TurnContext
{
    public bool HasSuccessfullyPerformedAction { get; set; }

    /// <summary>
    ///     Waiting player drag a card into a valid position on board
    /// </summary>
    public bool HasSelectedInitialBlock { get; set; }

    public ICard SelectedCard { get; set; }

    /// <summary>
    ///     Drop which card has been dropped into
    /// </summary>
    public BlockData TargetBlock { get; set; }

    /// <summary>
    ///     Block that are selected to trigger the card effect
    /// </summary>
    public BlockData FinalBlock { get; set; }

    public void Reset()
    {
        HasSuccessfullyPerformedAction = false;
        HasSelectedInitialBlock = false;

        SelectedCard = null;
        TargetBlock = null;
        FinalBlock = null;
    }
}