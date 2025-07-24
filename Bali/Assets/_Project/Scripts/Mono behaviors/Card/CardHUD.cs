using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardHUD : MonoBehaviour
{
    [Title("References")]
    [SerializeField]
    private TMP_Text nameLabel;

    [SerializeField]
    private TMP_Text classLabel;

    [SerializeField]
    private Image portrait;

    [SerializeField]
    private TMP_Text baseDamageLabel;

    [SerializeField]
    private TMP_Text baseHealthLabel;

    [SerializeField]
    private TMP_Text magickaNameLabel;

    [SerializeField]
    private TMP_Text magickaDescriptionLabel;

    [SerializeField]
    private TMP_Text bonusDamageLabel;

    [SerializeField]
    private TMP_Text bonusHealthLabel;

    [SerializeField]
    private TMP_Text redCostLabel;

    [SerializeField]
    private TMP_Text blackCostLabel;

    [SerializeField]
    private TMP_Text whiteCostLabel;
    
    [Button]
    [DisableInEditorMode]
    public void Setup (CardData data)
    {
        nameLabel.text = data.Name;
        classLabel.text = data.Class.ToString();
        portrait.sprite = data.Portrait;
        baseDamageLabel.text = $"{data.BaseDamage}";
        baseHealthLabel.text = $"{data.BaseHealth}";

        var skillData = data.Magicka;
        magickaNameLabel.text = skillData.Name;
        magickaDescriptionLabel.text = skillData.Description;
        bonusDamageLabel.text = $"{skillData.BonusDamage}";
        bonusHealthLabel.text = $"{skillData.BonusLife}";

        redCostLabel.text = $"{skillData.Cost.red}";
        blackCostLabel.text = $"{skillData.Cost.black}";
        whiteCostLabel.text = $"{skillData.Cost.white}";
    }

    public void UpdateCardHealth (int newHealth)
    {
        baseHealthLabel.text = $"{newHealth}";
    }
}