using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CancelAction : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Button button;

    private void Awake()
    {
        ServiceLocator.CancelAction = this;
        GameEntryPoints.OnFinishedSetup += _ => Hide();
    }

    public void SetCancelBehavior (Action cancelAction)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => cancelAction());
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}