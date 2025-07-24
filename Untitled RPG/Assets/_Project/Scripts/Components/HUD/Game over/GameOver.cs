using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger retryButton;

    [TitleGroup("References")]
    [SerializeField]
    private EventTrigger toCharacterCreationButton;

    private void Awake()
    {
        GameEvents.OnGameOver += PlayerGameOverListener;
        GameEvents.OnBattleFinishedEntryPoint += BattleFinishedListener;

        AddRetryBehavior();
        AddNewCharacterBehavior();
    }

    private IEnumerator BattleFinishedListener (BattleSetupContext arg)
    {
        if (Blackboard.GameInfo.RemainingTries <= 0)
            GameEvents.OnGameOver?.Invoke();

        yield break;
    }

    private void PlayerGameOverListener() => gameObject.SetActive(true);

    private void AddNewCharacterBehavior()
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => StartCoroutine(NewCharacterListener()));

        toCharacterCreationButton.triggers.Clear();
        toCharacterCreationButton.triggers.Add(entry);
    }

    private void AddRetryBehavior()
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => StartCoroutine(RetryListener()));

        retryButton.triggers.Clear();
        retryButton.triggers.Add(entry);
    }

    private IEnumerator NewCharacterListener()
    {
        yield return ServiceLocator.ScreenFading.FadeIn();
        gameObject.SetActive(false);
        GameEvents.OpenCharacterCreationMenu?.Invoke();
    }

    private IEnumerator RetryListener()
    {
        yield return ServiceLocator.ScreenFading.FadeIn();
        gameObject.SetActive(false);
        GameEvents.OnGameOverRetryButtonPressed?.Invoke();
    }
}