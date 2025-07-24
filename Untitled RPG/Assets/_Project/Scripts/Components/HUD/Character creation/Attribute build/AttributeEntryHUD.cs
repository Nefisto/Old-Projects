using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeEntryHUD : SerializedMonoBehaviour
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public AttributeType AttributeType { get; private set; }

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text valueLabel;

    [TitleGroup("References")]
    [SerializeField]
    private Button minusButton;

    [TitleGroup("References")]
    [SerializeField]
    private Button plusButton;

    public void Setup (TraitChart traitChart)
    {
        var attribute = traitChart
            .GetGameAttributeFromType(AttributeType);

        valueLabel.text = attribute.Grow.ToString();

        attribute.OnUpdatedGrow += grow => { valueLabel.text = grow.ToString(); };

        traitChart.OnUpdateSpendPoints += () =>
        {
            ValidateMinusButtons(attribute);
            ValidatePlusButton(attribute, traitChart);
        };

        ValidateMinusButtons(attribute);
        ValidatePlusButton(attribute, traitChart);

        minusButton.onClick.RemoveAllListeners();
        plusButton.onClick.RemoveAllListeners();

        minusButton.onClick.AddListener(attribute.DecreaseGrow);
        plusButton.onClick.AddListener(attribute.IncreaseGrow);
    }

    private void ValidateMinusButtons (Trait attribute) => minusButton.interactable = attribute.Grow > 1;

    private void ValidatePlusButton (Trait attribute, TraitChart traitChart)
        => plusButton.interactable = attribute.Grow < 9 && CanSpendMorePoints(traitChart);

    private bool CanSpendMorePoints (TraitChart trait)
        => Database.AccountData.PointsToDistribute - trait.SpentPoints > 0;
}