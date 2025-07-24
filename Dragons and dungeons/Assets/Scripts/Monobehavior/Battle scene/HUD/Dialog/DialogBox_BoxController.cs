using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class DialogBox
{
    public event Action OnOpening;
    
    [TabGroup("Basic", true)]
    [Tooltip("How many seconds this will take to increase horizontally")]
    [Title("Duration")]
    [SerializeField]
    private float increaseHorizontal = 1f;

    [TabGroup("Basic")]
    [Tooltip("How many seconds this will take to increase vertically")]
    [SerializeField]
    private float increaseVertical = 1f;

    [TabGroup("Basic")]
    [Tooltip("How many seconds this will take to decrease horizontally")]
    [SerializeField]
    private float decreaseHorizontal = 1f;

    [TabGroup("Basic")]
    [Tooltip("How many seconds this will take to decrease vertically")]
    [SerializeField]
    private float decreaseVertical = 1f;

    [TabGroup("Basic")]
    [Title("Animation")]
    [Tooltip("What percentage amount of height it will left appearing before start to reduce horizontally in close animation")]
    [MinValue(0f), MaxValue(1f)]
    [SerializeField]
    private float closeMinHeight = .15f;
    
    [TabGroup("Basic")]
    [Title("Control")]
    [SerializeField]
    private GameObject dialogBackground;

    [TabGroup("Basic")]
    [SerializeField]
    private GameObject dialogBox;
    
    [TabGroup("Basic")]
    [SerializeField]
    private GameObject dialogTile;

    [TabGroup("Basic")]
    [Tooltip("The invisible box that prevents player to click on game actions, and process any click as it was intended to dialog")]
    [SerializeField]
    private GameObject clickableBox;
    
    [TabGroup("Basic")]
    [Tooltip("Black image in background")]
    [SerializeField]
    private GameObject backgroundImage;

    [TabGroup("Basic")]
    [ReadOnly]
    [SerializeField]
    // To check when this read to type text
    private DialogBoxState currentState = DialogBoxState.Closed;

    // To allow us to finish the opening instantly
    private Sequence openingSequence;
    private Sequence closingSequence;
    
    private IEnumerator OpenDialogRoutine()
    {
        OnOpening?.Invoke();

        UnhideDialogBox();

        SetupDialog(dialogSpeakers.First());

        StartOpeningAnimation();

        yield return openingSequence.WaitForCompletion();

        openingSequence = null;
        
        ChangeDialogBoxState(DialogBoxState.Waiting);
    }

    private void OpenDialogInstantly()
        => openingSequence.Complete();

    private IEnumerator CloseDialogRoutine()
    {
        ChangeDialogBoxState(DialogBoxState.Closing);
        
        ResetMessages();

        StartClosingAnimation();

        yield return closingSequence.WaitForCompletion();
    }

    private void CloseDialogInstantly()
        => closingSequence.Complete();

    private void ChangeDialogBoxState(DialogBoxState boxState)
        => currentState = boxState;

    private void ResetMessages()
    {
        currentMessageIndex = -1;
        dialogSpeakers.Clear();
    }

    private void StartOpeningAnimation()
    {
        openingSequence = DOTween.Sequence();
        openingSequence.Append((dialogBox.transform as RectTransform).DOSizeDelta(new Vector2(0f, ((RectTransform)dialogBox.transform).sizeDelta.y), increaseHorizontal));
        openingSequence.Append((dialogBox.transform as RectTransform).DOSizeDelta(new Vector2(0f, 0f), increaseVertical));
        openingSequence.Append(bigSprite.DOFade(1f, 1f));
        openingSequence.Join(dialogName.DOFade(1f, 1f));
        openingSequence.OnComplete(() => ChangeDialogBoxState(DialogBoxState.Waiting));
    }

    private void StartClosingAnimation()
    {
        var closeSize = GetMinimumSizeToCloseAnimation();

        closingSequence = DOTween.Sequence();
        closingSequence.Append((dialogBox.transform as RectTransform).DOSizeDelta(new Vector2(0f, closeSize.y), decreaseVertical));
        closingSequence.Append((dialogBox.transform as RectTransform).DOSizeDelta(new Vector2(closeSize.x, closeSize.y), decreaseHorizontal));
        closingSequence.Append(bigSprite.DOFade(0f, 1f));
        closingSequence.Join(dialogName.DOFade(0f, 1f));
        closingSequence.OnComplete(HideDialogBox);
    }

    private void UnhideDialogBox()
    {
        // Show the clickable box
        clickableBox.SetActive(true);
        
        // Show the background
        backgroundImage.SetActive(true);
        
        // Show the big sprite
        bigSprite.gameObject.SetActive(true);

        // Show the dialog name
        dialogName.gameObject.SetActive(true);
        
        dialogBackground.SetActive(true);
        dialogTile.SetActive(true);
    }

    private void HideDialogBox()
    {
        // Hide the clickable box
        clickableBox.SetActive(false);
        
        // Hide the background
        backgroundImage.SetActive(false);
        
        // Hide big sprite
        bigSprite.color = new Color(bigSprite.color.r, bigSprite.color.g, bigSprite.color.b, 0f);
        bigSprite.gameObject.SetActive(false);
        
        // Hide dialog name
        dialogName.color = new Color(dialogName.color.r, dialogName.color.g, dialogName.color.b, 0f);
        dialogName.gameObject.SetActive(false);

        // Reduce the size
        ((RectTransform)dialogBox.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        ((RectTransform)dialogBox.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);

        // Clear the text
        textPanel.text = "";
        dialogName.text = "";
        
        // Set state
        currentState = DialogBoxState.Closed;

        // HIDE GO
        dialogBackground.SetActive(false);
        dialogTile.SetActive(false);
    }

    private Vector2 GetMinimumSizeToCloseAnimation()
    {
        var sizeBetweenAnchors = ((RectTransform)dialogBox.transform).NTGetSizeBetweenAnchor();
        var minHeightMultiplier = 1f - closeMinHeight;
        
        return new Vector2(-sizeBetweenAnchors.x, -sizeBetweenAnchors.y * minHeightMultiplier);
    }
}