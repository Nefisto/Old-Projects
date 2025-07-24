using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Control behavior of selected/unselected state
/// </summary>
[SelectionBase]
public partial class ActionPanel
{
    [TabGroup("State")]
    [Title("Settings")]
    [OnValueChanged("SetPanelAlphaState")]
    [SerializeField]
    private ActionPanelState state = ActionPanelState.Unselected;

    [TabGroup("State")]
    [Tooltip("Alpha used when character is the turn owner")]
    [Range(0f, 1f)]
    [SerializeField]
    public float selectedAlpha = .45f;

    [TabGroup("State")]
    [Tooltip("Alpha used when character ISN'T the turn owner")]
    [Range(0f, 1f)]
    [SerializeField]
    private float unselectedAlpha = .2f;

    /// <summary>
    /// When isn't the character turn, their panel should be more transparent than the turn owner 
    /// </summary>
    public void SetPanelAlphaState (ActionPanelState newState)
    {
        // Change alpha
        var newAlpha = newState == ActionPanelState.Selected ? selectedAlpha : unselectedAlpha;
        state = newState;
        ChangeChildrenAndMyselfAlpha(newAlpha);

        // Disable/Enable buttons
    }

    /// <summary>
    /// To simplify the call on editor only
    /// </summary>
    [TabGroup("State")]
    [PropertyOrder(1)]
    [Button]
    private void ApplyPanelAlphaState()
        => SetPanelAlphaState(state);

    private void ChangeChildrenAndMyselfAlpha (float newAlpha)
    {
        foreach (var image in GetComponentsInChildren<Image>())
        {
            if (image.gameObject == gameObject)
                continue;

            var actualAlpha = newAlpha;
            if (image.TryGetComponent<AlphaLevel>(out var alphaLevel))
            {
                actualAlpha = state == ActionPanelState.Selected 
                    ? alphaLevel.selectedAlpha 
                    : alphaLevel.notSelectedAlpha;
            }
            
            var color = image.color;
            image.color = new Color(color.r, color.b, color.b, actualAlpha);
        }
    }
}