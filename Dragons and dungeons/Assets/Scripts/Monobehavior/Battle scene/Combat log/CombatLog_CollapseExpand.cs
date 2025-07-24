using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public partial class CombatLog
{
    [TabGroup("Collapse Expand")]
    [Title("Settings")]
    [SerializeField]
    private float collapsedSize = 40f;

    [TabGroup("Collapse Expand")]
    [SerializeField]
    private float expandedSize = 180f;

    [TabGroup("Collapse Expand")]
    [Title("Control")]
    [SerializeField]
    private RectTransform scrollView;

    [TabGroup("Collapse Expand")]
    [SerializeField]
    private Button collapseButton;

    [TabGroup("Collapse Expand")]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool isCollapsed;

    private ScrollRect scrollRect;

    private void SetupCollapse()
    {
        scrollRect = scrollView.GetComponent<ScrollRect>();
        Collapse();
    }

    [UsedImplicitly]
    public void ToggleCollapse()
    {
        if (isCollapsed)
            Expand();
        else
            Collapse();
    }

    private void Collapse()
    {
        scrollView
            .DOSizeDelta(new Vector2(0f, collapsedSize), .5f)
            .OnStart(() =>
            {
                ResetViewToLastLogEntry();
                DisableButton();
                DisableVerticalDrag();
            })
            .OnComplete(() =>
            {
                EnableButton();
                isCollapsed = true;
            });
    }

    private void Expand()
    {
        scrollView
            .DOSizeDelta(new Vector2(0f, expandedSize), .5f)
            .OnStart(DisableButton)
            .OnComplete(() =>
            {
                EnableButton();
                EnableVerticalDrag();
                isCollapsed = false;
            });
    }

    private void ResetViewToLastLogEntry()
        => contentFolder.anchoredPosition = Vector2.zero;

    private void EnableButton()
        => collapseButton.interactable = true;

    private void DisableButton()
        => collapseButton.interactable = false;

    private void EnableVerticalDrag()
        => scrollRect.vertical = true;

    private void DisableVerticalDrag()
        => scrollRect.vertical = false;
}