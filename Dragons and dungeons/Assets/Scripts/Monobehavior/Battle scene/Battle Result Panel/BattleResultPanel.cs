using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultPanel : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private GameObject battleResultPanel;

    [SerializeField]
    private GameObject beautyPointGroup;

    [SerializeField]
    private TextMeshProUGUI beautyPointValue;

    [SerializeField]
    private GameObject madnessPointGroup;

    [SerializeField]
    private TextMeshProUGUI madnessPointValue;

    [SerializeField]
    private GameObject experiencePointGroup;

    [SerializeField]
    private TextMeshProUGUI experiencePointValue;

    [Space]
    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private FadeBackground background;
    
    private void Start()
        => HideMainPanel();

    private void OnEnable()
        => GameEvents.Battle.OnSetupBattle += SetupBattleListener;

    private void OnDisable()
        => GameEvents.Battle.OnSetupBattle -= SetupBattleListener;

    private void SetupBattleListener (BattleEncounterContext ctx)
    {
        HideMainPanel();
        ResetPoints();
        EnableContinueButton();
        SetupContinueButton(ctx);
    }

    private void SetupContinueButton (BattleEncounterContext ctx)
    {
        continueButton.onClick.AddListener(() =>
        {
            StartCoroutine(BackToDungeonRoutine());

            DisableContinueButton();
        });

        IEnumerator BackToDungeonRoutine()
        {
            yield return background.FadeInRoutine();

            ctx.ReturnToDungeonMethod?.Invoke();
        }
    }

    public IEnumerator ShowBattleResult (BattleResultContext ctx)
    {
        HideGroups();
        ResetPoints();
        DisableContinueButton();
        ShowMainPanel();

        ShowBeautyGroup();
        yield return NumberChangeRoutine(beautyPointValue, ctx.BeautyPoints);

        ShowMadnessGroup();
        yield return NumberChangeRoutine(madnessPointValue, ctx.MadnessPoints);

        ShowExperienceGroup();
        yield return NumberChangeRoutine(experiencePointValue, ctx.ExperiencePoints);

        yield return new WaitForSeconds(.5f);

        EnableContinueButton();
    }

    private void EnableContinueButton()
        => continueButton.gameObject.SetActive(true);

    private void DisableContinueButton()
        => continueButton.gameObject.SetActive(false);

    private IEnumerator NumberChangeRoutine (TextMeshProUGUI tmp, int target, float frameDuration = 60f)
    {
        var jumpAmount = 1 + target / frameDuration;
        var counter = 0;
        while (counter < target)
        {
            tmp.text = $"{counter}";

            counter = (int)Mathf.Clamp(counter + jumpAmount, 0, target);
            yield return null;
        }
    }

    private void ShowBeautyGroup()
        => beautyPointGroup.SetActive(true);

    private void ShowMadnessGroup()
        => madnessPointGroup.SetActive(true);

    private void ShowExperienceGroup()
        => experiencePointGroup.SetActive(true);

    private void HideMainPanel()
        => battleResultPanel.SetActive(false);

    private void ShowMainPanel()
        => battleResultPanel.SetActive(true);

    private void HideGroups()
    {
        beautyPointGroup.SetActive(false);
        madnessPointGroup.SetActive(false);
        experiencePointGroup.SetActive(false);
    }

    private void ResetPoints()
    {
        beautyPointValue.text = "0";
        madnessPointValue.text = "0";
        experiencePointValue.text = "0";
    }
    
#if UNITY_EDITOR
    [Button]
    public void ShowPanel()
        => StartCoroutine(ShowBattleResult(new BattleResultContext
        {
            battleResult = new BattleResult()
            {
                beautyPoints = 38,
                madnessPoints = 78,
                experiencePoints = 51
            }
        }));
#endif
}