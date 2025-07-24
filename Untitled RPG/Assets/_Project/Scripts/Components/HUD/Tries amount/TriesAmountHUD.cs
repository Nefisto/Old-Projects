using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public partial class TriesAmountHUD : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text triesRemainingLabel;

    private void Awake()
    {
        GameEvents.OnSetupAdventure += () =>
        {
            Blackboard.GameInfo.RemainingTries = GameConstants.INITIAL_TRIES_AMOUNT;
            StartCoroutine(RefreshTriesAmountLabel());
        };
        GameEvents.OnBattleFinishedEntryPoint += _ => StartCoroutine(RefreshTriesAmountLabel());

        GameEvents.OnBattleFinishingdEntryPoint += _
            => Blackboard.GameInfo.RemainingTries -=
                ServiceLocator.BattleResulData.Winner == BattleResultData.BattleWinner.Player
                    ? GameConstants.TRIES_LOST_WHEN_WIN
                    : GameConstants.TRIES_LOST_WHEN_LOSE;

#if UNITY_EDITOR
        GameEvents.OnUpdateGameTries += () => StartCoroutine(RefreshTriesAmountLabel());
#endif
    }

    private IEnumerator RefreshTriesAmountLabel()
    {
        triesRemainingLabel.text = $"{Mathf.Max(Blackboard.GameInfo.RemainingTries, 0)}";
        yield break;
    }
}