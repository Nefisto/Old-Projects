namespace Loot.Enum
{
    /// <summary>
    ///     Decide how Drop Table will calculate the percentage
    /// </summary>
    public enum PercentageCalculation
    {
        /// <summary>
        ///     Percentages will be independent
        /// </summary>
        Simple,

        /// <summary>
        ///     Percentages will be calculated based on total weighted of all drops in table
        /// </summary>
        Weighted
    }
}