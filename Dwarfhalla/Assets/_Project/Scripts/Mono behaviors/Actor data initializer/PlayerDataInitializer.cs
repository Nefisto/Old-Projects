using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerDataInitializer : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private List<Card> initialDeck;

    [TitleGroup("References")]
    [SerializeField]
    private List<Card> fixedCards;

    [ShowInInspector]
    private PlayerData playerData;

    private void Awake()
    {
        GameEntryPoints.GeneratingSessionData += DataHandle;
    }

    private IEnumerator DataHandle (object _)
    {
        ServiceLocator.GameContext.PlayerData = new PlayerData(new PlayerData.PlayerDataSettings
        {
            deck = initialDeck
                .Select(c => c.GetInstance)
                .ToList(),
            fixedCards = fixedCards
                .Select(c => c.GetInstance)
                .ToList()
        });

        playerData = ServiceLocator.GameContext.PlayerData;
        GameEntryPoints.OnSelectedReward += (sender, args)
            =>
        {
            if (args is not RewardScreen.SelectedRewardEventArgs correctArgs)
                return;

            if (correctArgs.Reward == null)
                return;

            playerData.Coins.Value -= correctArgs.Reward.Price;
            playerData.Deck.AddToDeck(correctArgs.Reward);
        };

        yield return GameEntryPoints.OnGeneratedPlayerData?.YieldableInvoke();
    }
}