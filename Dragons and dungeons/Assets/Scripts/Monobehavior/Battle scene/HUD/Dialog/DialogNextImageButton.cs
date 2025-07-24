using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Button that appear when the dialog is finished
/// </summary>
public class DialogNextImageButton : LazyBehavior
{
    [Title("Control")]
    [SerializeField]
    private DialogBox dialogBox;

    private void Awake()
    {
        dialogBox.OnOpening += HideButtonFeedback;
        dialogBox.OnStartMessage += HideButtonFeedback;
        dialogBox.OnFinishMessage += ShowButtonFeedback;
    }

    private void ShowButtonFeedback()
        => image.enabled = true;

    private void HideButtonFeedback()
        => image.enabled = false;
}