using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class OpenStatusMenu : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private StatusMenuView statusMenuView;

    [TitleGroup("Settings")]
    [SerializeField]
    private Button button;

    private void Awake()
    {
        GameEvents.onBattleTriggered += _ => button.interactable = false;
        GameEvents.OnBattleFinishedEntryPoint += _ => button.interactable = true;

        button.onClick.AddListener(() => StartCoroutine(Open()));
    }

    private IEnumerator Open()
    {
        var characterData = ServiceLocator.SessionManager.PlayableCharacterData;
        yield return statusMenuView.Setup(characterData);
        ServiceLocator.MenuStack.OpenMenu(statusMenuView);
    }
}