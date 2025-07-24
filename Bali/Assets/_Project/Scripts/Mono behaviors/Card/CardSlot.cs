using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class CardSlot : MonoBehaviour
{
    [Title("References")]
    [SerializeField]
    private CardHUD cardHUD;

    [ReadOnly]
    [OdinSerialize]
    public CardData CardData { get; private set; }

    public string CardName => CardData.Name;
    public bool IsAlive => CardData != null && CardData.IsAlive();
    public void Setup (CardData cardData)
    {
        if (CardData != null)
            RemoveListeners();
        
        CardData = cardData;
        if (cardData != null && cardData.IsAlive())
        {
            cardHUD.Setup(cardData);
            AddListeners();
            ShowCardHUD();
        }
        else
        {
            HideCardHUD();
        }
    }

    private void AddListeners()
        => CardData.OnUpdateLife += cardHUD.UpdateCardHealth;

    private void RemoveListeners()
        => CardData.OnUpdateLife -= cardHUD.UpdateCardHealth;

    public bool IsEmptySlot()
        => CardData == null;

    public void ClearSlot()
        => CardData = null;
    
    public void ShowCardHUD()
        => cardHUD.gameObject.SetActive(true);

    public void HideCardHUD()
        => cardHUD.gameObject.SetActive(false);

    public void Change (CardSlot otherSlot)
    {
        var otherData = otherSlot.CardData;
        otherSlot.Setup(CardData);
        Setup(otherData);
    }
}