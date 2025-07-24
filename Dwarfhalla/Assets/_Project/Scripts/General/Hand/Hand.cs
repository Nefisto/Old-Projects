using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public partial class Hand : IEnumerable<ICard>
{
    [TitleGroup("Debug")]
    [ShowInInspector]
    private List<ICard> cards;

    public Hand (params ICard[] initialCards) => cards = new List<ICard>(initialCards);

    public void AddCard (ICard card) { }

    public void RemoveCard (ICard card) => cards.Remove(card);
}