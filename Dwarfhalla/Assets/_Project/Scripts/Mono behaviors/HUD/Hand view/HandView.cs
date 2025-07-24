using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [TitleGroup("Prefabs")]
    [SerializeField]
    private CardView cardViewPrefab;

    [TitleGroup("Prefabs")]
    [SerializeField]
    private CardView constantCardViewPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private Transform dwarfSummonCards;

    [TitleGroup("References")]
    [SerializeField]
    private Transform goblinSummonCards;

    [TitleGroup("References")]
    [SerializeField]
    private Transform allDwarfsSummonCards;

    [TitleGroup("References")]
    [SerializeField]
    private Transform constantDeckFolder;

    [TitleGroup("References")]
    [SerializeField]
    private DragIcon dragIcon;

    [TitleGroup("References")]
    [SerializeField]
    private Database database;

    private readonly List<CardView> debugCardViews = new();

    private readonly List<CardView> handCardViews = new();

    private PlayerData playerData;

    private void Awake()
    {
        ServiceLocator.HandView = this;
        GameEntryPoints.OnFinishedSetup += FinishSetupHandle;
        GameEntryPoints.OnPlayerPerformedAction += RefreshHand;
    }

    private void Start()
    {
        dwarfSummonCards.DestroyChildren();
        goblinSummonCards.DestroyChildren();
        constantDeckFolder.DestroyChildren();
        allDwarfsSummonCards.DestroyChildren();
    }

    public void DisableDragIcon() => dragIcon.DisableFollowingRoutine();

    public IEnumerator DisableCardsDrag()
    {
        foreach (var card in handCardViews)
            yield return card.DisableDrag();
    }

    public IEnumerator EnableCardsDrag()
    {
        foreach (var cardView in handCardViews)
        {
            yield return playerData.ActionPoints.CurrentPoints >= cardView.CardData.Cost
                ? cardView.EnabledDrag()
                : cardView.DisableDrag();
        }
    }

    private IEnumerator SetupHandle (object arg)
    {
        yield return CreateHand();
        yield return CreateFixedHand();
        // Hidden hand containing all game cards
        yield return CreateDebugHand();
    }

    private IEnumerator RefreshHand (object arg)
    {
        var playedCard = ServiceLocator.GameContext.TurnContext.SelectedCard;

        if (playedCard is not SummonCard)
            yield break;

        var foundCardView = handCardViews.FirstOrDefault(c => c.CardData == playedCard);
        foundCardView?.HideCard();
        handCardViews.Remove(foundCardView);
    }

    public IEnumerator CreateHand()
    {
        handCardViews.Clear();
        dwarfSummonCards.DestroyChildren();
        foreach (var card in playerData.Hand)
        {
            var cardView = Instantiate(cardViewPrefab, dwarfSummonCards);
            cardView.OnDragBegin += () => CardBeginDragHandle(cardView);
            cardView.OnPointerExit += () =>
            {
                if (ServiceLocator.GameContext.TurnContext.SelectedCard == card)
                    dragIcon.EnableFollowingRoutine();
            };

            cardView.Setup(card);

            handCardViews.Add(cardView);
        }

        yield break;
    }

    private IEnumerator CreateFixedHand()
    {
        foreach (var card in playerData.FixedCards)
        {
            var cardView = Instantiate(constantCardViewPrefab, constantDeckFolder);
            cardView.OnDragBegin += () => CardBeginDragHandle(cardView);
            cardView.OnPointerExit += () =>
            {
                if (ServiceLocator.GameContext.TurnContext.SelectedCard == card)
                    dragIcon.EnableFollowingRoutine();
            };

            cardView.Setup(card);

            handCardViews.Add(cardView);
        }

        yield break;
    }

    private IEnumerator CreateDebugHand()
    {
        foreach (var card in database.Cards.Where(c
                     => c is SummonCard summonCard && summonCard.UnitData.UnitSide == UnitSide.Goblin))
        {
            var cardView = Instantiate(cardViewPrefab, goblinSummonCards);
            cardView.OnDragBegin += () => CardBeginDragHandle(cardView);
            cardView.OnPointerExit += () =>
            {
                if ((Card)ServiceLocator.GameContext.TurnContext.SelectedCard == card)
                    dragIcon.EnableFollowingRoutine();
            };

            cardView.Setup(card);

            debugCardViews.Add(cardView);
        }

        foreach (var card in database.Cards.Where(c
                     => c is SummonCard summonCard && summonCard.UnitData.UnitSide == UnitSide.Dwarf))
        {
            var cardView = Instantiate(cardViewPrefab, allDwarfsSummonCards);
            cardView.OnDragBegin += () => CardBeginDragHandle(cardView);
            cardView.OnPointerExit += () =>
            {
                if ((Card)ServiceLocator.GameContext.TurnContext.SelectedCard == card)
                    dragIcon.EnableFollowingRoutine();
            };

            cardView.Setup(card);

            debugCardViews.Add(cardView);
        }

        goblinSummonCards.gameObject.SetActive(false);
        allDwarfsSummonCards.gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator FinishSetupHandle (object arg)
    {
        playerData = ServiceLocator.GameContext.PlayerData as PlayerData;
        yield return SetupHandle(arg);
    }

    private void CardBeginDragHandle (CardView cardClicked)
    {
        ServiceLocator
            .GameContext
            .TurnContext
            .SelectedCard = cardClicked.CardData;
    }
}