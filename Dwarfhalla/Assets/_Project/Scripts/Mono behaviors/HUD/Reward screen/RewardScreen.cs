using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreen : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private CardViewOnReward rewardCardPrefab;

    [TitleGroup("References")]
    [SerializeField]
    private Transform rewardCardsFolder;

    [TitleGroup("References")]
    [SerializeField]
    private Button confirmButton;

    private readonly List<CardViewOnReward> instanceCards = new();

    private Canvas canvas;

    private RewardContext rewardContext;
    private CardViewOnReward selectedCard;

    private void Awake()
    {
        gameObject.SetActive(false);
        canvas = GetComponentInParent<Canvas>();
    }

    public IEnumerator RewardRoutine (RewardContext context)
    {
        gameObject.SetActive(true);

        rewardContext = context;

        rewardCardsFolder.DestroyChildren();
        selectedCard = null;
        confirmButton.interactable = false;

        yield return ServiceLocator.BackgroundImage.ShowImage(new BackgroundImage.Settings()
        {
            blockRaycast = true,
            alphaToSet = 0.5f,
            sortingLayerID = canvas.sortingLayerID,
            orderInLayer = canvas.sortingOrder - 1
        });

        yield return SpawnCards(context);
        yield return WaitForConfirmToBePressed();
        yield return GameEntryPoints.OnSelectedReward?.YieldableInvoke(this,
            new SelectedRewardEventArgs() { Reward = selectedCard?.Card });

        yield return ServiceLocator.BackgroundImage.HideImage();
        gameObject.SetActive(false);
    }

    private IEnumerator WaitForConfirmToBePressed()
    {
        var hasSelectedCard = false;
        confirmButton.interactable =
            rewardContext.rewards.All(c => c.Price > ServiceLocator.GameContext.PlayerData.Coins.Value);
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => hasSelectedCard = true);
        yield return new WaitUntil(() => hasSelectedCard);
    }

    private IEnumerator SpawnCards (RewardContext context)
    {
        instanceCards.Clear();

        foreach (var rewardCard in context.rewards)
        {
            var instance = Instantiate(rewardCardPrefab, rewardCardsFolder, false);
            instance.SetupRewardCard(rewardCard);
            instance.OnClick += () => SelectCard(instance);

            instanceCards.Add(instance);
        }

        yield break;
    }

    private void SelectCard (CardViewOnReward cardClicked)
    {
        instanceCards.ForEach(c => c.UnselectCard());

        selectedCard = cardClicked;
        selectedCard.SelectCard();
        confirmButton.interactable = true;
    }

    public class RewardContext
    {
        public List<SummonCard> rewards;
    }

    public class SelectedRewardEventArgs : EventArgs
    {
        public SummonCard Reward { get; set; }
    }
}