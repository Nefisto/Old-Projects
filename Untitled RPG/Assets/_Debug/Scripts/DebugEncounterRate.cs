using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugEncounterRate : MonoBehaviour
{
    public EncounterRateSystem ers;
    public TMP_Text percentageText;
    public Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(SetEncounterRate);
        EncounterRateSystem.OnEncounterRateUpdate += UpdatePercentage;

        SetEncounterRate(slider.value);
    }

    public void CheckBattle() => ers.CheckBattle();

    private void UpdatePercentage (EncounterRateUpdateContext ctx)
    {
        slider.SetValueWithoutNotify(ctx.resourcePercentage);
        UpdatePercentageText(ctx);
    }

    private void SetEncounterRate (float percentage) => ers.SetEncounterRate(percentage);

    private void UpdatePercentageText (EncounterRateUpdateContext ctx)
        => percentageText.text = $"{ctx.battleTriggerPercentage * 100:F2}%";
}