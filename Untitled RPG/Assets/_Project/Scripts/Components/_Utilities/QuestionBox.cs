using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class QuestionBox : MonoBehaviour, IMenu
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text label;

    [TitleGroup("References")]
    [SerializeField]
    private Button cancelButton;

    [TitleGroup("References")]
    [SerializeField]
    private Button confirmButton;

    private Action cancelOperation;

    private void Awake() => ServiceLocator.QuestionBox = this;

    public void Close (IMenu.CloseContext context = default)
    {
        if (context is { hasClosedThroughOutsideClick: true })
            cancelOperation?.Invoke();

        transform.gameObject.SetActive(false);
    }

    public IEnumerator Setup (Context context)
    {
        label.text = context.label;

        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();

        cancelButton.onClick.AddListener(() =>
        {
            context.cancelOperation?.Invoke();
            ServiceLocator.MenuStack.CloseMenu();
        });
        confirmButton.onClick.AddListener(() =>
        {
            context.confirmOperation?.Invoke();
            ServiceLocator.MenuStack.CloseMenu();
        });

        cancelOperation = context.cancelOperation;
        yield break;
    }

    public class Context
    {
        public Action cancelOperation;
        public Action confirmOperation;
        public string label;
    }
}