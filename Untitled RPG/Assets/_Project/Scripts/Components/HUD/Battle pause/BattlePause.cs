using Sirenix.OdinInspector;
using UnityEngine;

public class BattlePause : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private InformationTab informationTab;

    private void Awake()
    {
        GameEvents.OnPause += ShowMenu;
        GameEvents.OnUnpause += HideMenu;
        SessionManager.OnLoadedPlayerCharacterData += characterData =>
        {
            informationTab.Setup(characterData);
            HideMenu();
        };
    }

    private void ShowMenu()
    {
        informationTab.Refresh(ServiceLocator.SessionManager.PlayableCharacterData);
        gameObject.SetActive(true);
    }

    private void HideMenu() => gameObject.SetActive(false);
}