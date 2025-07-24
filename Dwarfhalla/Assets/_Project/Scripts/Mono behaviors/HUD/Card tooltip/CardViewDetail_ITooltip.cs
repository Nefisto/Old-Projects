using NTools;
using UnityEngine;

public partial class CardViewDetail
{
    public void ShowTooltip()
    {
        gameObject.SetActive(true);
        mouseFollowRoutine = new NTask(SetPositionOnMouse());
        // mouseFollowRoutine?.Resume();
    }

    public void HideTooltip()
    {
        mouseFollowRoutine?.Stop();
        gameObject.SetActive(false);
    }

    public void Setup (ICard card)
    {
        if (card is not SummonCard summonCard)
        {
            Debug.LogWarning("Trying to show a card that isn't a summon card on tooltip");
            return;
        }

        attackPatternImage.sprite = summonCard.PatternImage;
        rangeLabel.text = $"Range: {summonCard.Range}";
        targetLabel.text = summonCard.DamageType.ToLabel();
        descriptionLabel.text = summonCard.UnitData.Description;
        lifeLabel.text = summonCard.UnitData.MaxHealth.ToString();
        damageLabel.text = summonCard.UnitData.Damage.ToString();
    }
}