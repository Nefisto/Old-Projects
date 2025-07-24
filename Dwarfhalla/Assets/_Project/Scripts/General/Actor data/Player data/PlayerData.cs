using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerData : IPlayerData
{
    [field: HideReferenceObjectPicker]
    [field: SerializeField]
    public Observable<int> Coins { get; set; } = new();

    public PlayerData (PlayerDataSettings settings)
    {
        Deck = new Deck(settings.deck.ToArray());
        FixedCards = settings.fixedCards;

        Hand = new Hand(Deck.GetInitialCards().ToArray());
        ActionPoints = new ActionPoints() { MaxPoints = 60 };

        GameEntryPoints.OnGoblinDied += GoblinDieHandle;
    }

    public List<ICard> FixedCards { get; set; }

    [ShowInInspector]
    public Deck Deck { get; set; }

    public Hand Hand { get; set; }
    public ActionPoints ActionPoints { get; }

    public List<SummonCard> GetAllSummonCards
        => Deck
            .OfType<SummonCard>()
            .Concat(Hand
                .OfType<SummonCard>())
            .ToList();

    private IEnumerator GoblinDieHandle (object arg)
    {
        var goblinData = (UnitData)arg;

        Coins.Value += goblinData.CoinReward;

        yield break;
    }

    public void DiscardHand() => Deck.AddToDiscardPile(Hand.ToArray());
    public void CreateNewHand() => Hand = new Hand(Deck.GetNewHand().ToArray());

    public class PlayerDataSettings
    {
        public List<ICard> deck;
        public List<ICard> fixedCards;
    }
}