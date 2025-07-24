using System;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;

[Serializable]
public partial class Deck : IEnumerable<ICard>
{
    [ShowInInspector]
    private List<ICard> cards = new();

    [ShowInInspector]
    private List<ICard> discardDeck = new();

    public Deck (params ICard[] cards) => this.cards = cards.ToList();

    public List<ICard> GetNewHand()
    {
        var result = cards
            .Take(4)
            .ToList();

        if (result.Count != 4)
        {
            ShuffleDiscardIntoDeck();
            result.AddRange(cards.Take(4 - result.Count));
        }

        foreach (var card in result)
            cards.Remove(card);

        return result;
    }

    private void ShuffleDiscardIntoDeck()
    {
        cards = discardDeck.Shuffle().ToList();
        discardDeck.Clear();
    }

    public void AddToDiscardPile (params ICard[] cards)
    {
        if (cards == null || cards.Length == 0)
            return;

        discardDeck.AddRange(cards);
    }

    public void AddToDeck (params ICard[] card)
    {
        if (card == null || card.Length == 0)
            return;

        cards.AddRange(card);
    }

    public List<ICard> GetInitialCards()
        => new()
        {
            GetSpecificCard("Axeman"),
            GetSpecificCard("Axeman"),
            GetSpecificCard("Axeman"),
            GetSpecificCard("Berserker")
        };

    private ICard GetNextCard()
    {
        var foundCard = cards.FirstOrDefault();
        cards.Remove(foundCard);

        return foundCard;
    }

    private ICard GetSpecificCard (string name)
    {
        var foundCard = cards.FirstOrDefault(c => c.Name == name);
        cards.Remove(foundCard);

        return foundCard;
    }

    public class DeckSetupContext
    {
        public List<ICard> Cards { get; set; }
    }
}