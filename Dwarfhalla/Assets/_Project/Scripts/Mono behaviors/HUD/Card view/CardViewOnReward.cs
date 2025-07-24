using System;
using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardViewOnReward : MonoBehaviour
{
    private const string RangeTemplate = "Range: {0}";

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text nameLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text priceLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text costLabel;

    [TitleGroup("References")]
    [SerializeField]
    private Image iconImage;

    [TitleGroup("References")]
    [SerializeField]
    private Image patternImage;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text rangeLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text damageTypeLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text descriptionLabel;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger eventTrigger;

    [TitleGroup("References")]
    [SerializeField]
    private Image background;

    private bool isDisabled = false;

    public Action OnClick;

    public SummonCard Card { get; private set; }

    public void SetupRewardCard (SummonCard cardData)
    {
        Card = cardData;

        nameLabel.text = cardData.Name;
        costLabel.text = cardData.Cost.ToString();
        priceLabel.text = cardData.Price.ToString();
        iconImage.sprite = cardData.Icon;
        patternImage.sprite = cardData.PatternImage;
        rangeLabel.text = string.Format(RangeTemplate, cardData.Range);
        damageTypeLabel.text = cardData.DamageType.ToLabel();
        descriptionLabel.text = cardData.UnitData.Description;

        if (ServiceLocator.GameContext.PlayerData.Coins.Value < cardData.Price)
        {
            DisableCard();
            return;
        }

        SetupMouseInteraction();
    }

    public void SelectCard()
    {
        if (isDisabled)
            return;

        background.color = Color.yellow.SetAlpha(background.color.a);
    }

    public void UnselectCard()
    {
        if (isDisabled)
            return;

        background.color = Color.white.SetAlpha(background.color.a);
    }

    private void DisableCard()
    {
        foreach (var image in GetComponentsInChildren<Image>())
            image.color = Color.gray;

        foreach (var text in GetComponentsInChildren<TMP_Text>())
            text.color = text.color.SetAlpha(0.35f);

        isDisabled = true;
    }

    private void SetupMouseInteraction()
    {
        var clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => { OnClick?.Invoke(); });

        eventTrigger.triggers.Add(clickEntry);
    }
}