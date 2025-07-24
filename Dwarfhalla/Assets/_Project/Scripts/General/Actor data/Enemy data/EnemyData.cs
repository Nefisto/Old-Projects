using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

[Serializable]
public class EnemyData : IPlayerData
{
    [ShowInInspector]
    public Deck Deck { get; set; } = new();

    public ActionPoints ActionPoints { get; } = new() { MaxPoints = 4 };

    [ShowInInspector]
    public Hand Hand { get; set; } = new();

    public List<SummonCard> GetAllSummonCards
        => Deck
            .OfType<SummonCard>()
            .Concat(Hand
                .OfType<SummonCard>())
            .ToList();

    public void DiscardHand() => Deck.AddToDiscardPile(Hand.ToArray());

    public void DiscardCard (ICard card)
    {
        Hand.RemoveCard(card);
        Deck.AddToDiscardPile(card);
    }

    public void RemoveCardFromHand (ICard card) => Hand.RemoveCard(card);

    public void CreateNewHand() => Hand = new Hand(Deck.GetNewHand().ToArray());
}