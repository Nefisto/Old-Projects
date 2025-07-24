using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[SelectionBase]
public partial class BattleResultHUD : MonoBehaviour, IBattleResultHUD
{
    [TitleGroup("References")]
    [SerializeField]
    private ExperienceCounterHUD experienceCounter;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text currencyAmount;


    [TitleGroup("References")]
    [SerializeField]
    private InteractionPanel detectionPanel;

    private void Awake()
    {
        ServiceLocator.BattleResultHUD = this;

        GameEvents.OnBattleFinishedEntryPoint += _ => gameObject.SetActive(false);
    }

    public void Setup (BattleSetupContext ctx)
    {
        gameObject.SetActive(false);
        ((RectTransform)transform).anchoredPosition = Vector2.zero;

        detectionPanel.Setup(_ => experienceCounter.shouldFinishNextFrame = true);
    }

    public IEnumerator Run (BattleResultData resultData)
    {
        gameObject.SetActive(true);

        var totalCurrency = resultData.GetTotalCurrency();

        currencyAmount.text = $"{totalCurrency}";
        yield return experienceCounter.Run(resultData, .75f);

        ServiceLocator.SessionManager.PlayableCharacterData.AddExperience(resultData.GetTotalExp());
        ServiceLocator.SessionManager.PlayableCharacterData.AddCurrency(totalCurrency);
    }
}