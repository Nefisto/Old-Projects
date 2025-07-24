using System.Collections.Generic;
using System.Linq;

/// <summary>
///     Common for both sided of the battle, player and enemy data
/// </summary>
public interface IPlayerData
{
    public Hand Hand { get; }
    public Deck Deck { get; }

    public ActionPoints ActionPoints { get; }

    public List<SummonCard> GetAllSummonCards { get; }
}