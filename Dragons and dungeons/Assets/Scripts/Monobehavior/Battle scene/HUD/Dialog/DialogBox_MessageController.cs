using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class DialogBox
{
    public event Action OnStartMessage;
    public event Action OnFinishMessage;

    [TabGroup("Message")]
    [Title("Settings")]
    [SerializeField]
    private int lettersPerSecond = 1;

    [TabGroup("Message")]
    [Title("Control")]
    [SerializeField]
    private TextMeshProUGUI textPanel;

    [TabGroup("Message")]
    [SerializeField]
    private Image dialogIcon;

    [TabGroup("Message")]
    [SerializeField]
    private Image bigSprite;

    [TabGroup("Message")]
    [SerializeField]
    private TextMeshProUGUI dialogName;

    [TabGroup("Message")]
    [SerializeField]
    private TextMeshProUGUI lifeText;

    [TabGroup("Message")]
    [Title("Debug")]
    [SerializeField]
    private List<DialogContext> dialogSpeakers;

    private int currentMessageIndex = -1;
    private bool userClickedOnScreen = false;

    private IEnumerator writeRoutine;
    private IEnumerator nextRoutine;
    private IEnumerator openingRoutine;
    private IEnumerator dialogRoutine;

    private void SetupDialog (DialogContext speaker)
    {
        dialogIcon.sprite = speaker.icon;
        bigSprite.sprite = speaker.bigSprite;
        dialogName.text = speaker.name;
        lifeText.text = speaker.life;
    }

    public void AdvanceDialog()
        => userClickedOnScreen = true;

    public void AddDialog (DialogContext dialog)
        => dialogSpeakers.Add(dialog);

    [TabGroup("Message")]
    [DisableInEditorMode]
    [Button]
    public void TestDialog (EnemyData enemy)
    {
        if (enemy == null)
            enemy = (EnemyData)BattleManager.Instance.GetEnemiesCharacters().First().Data;
        
        var dialog = enemy
            .GetDialogs(DialogCommonEvents.Start)
            .First();

        foreach (var message in dialog)
        {
            AddDialog(new DialogContext()
            {
                icon = enemy.icon,
                life = "--/--",
                bigSprite = dialog.BigSprite,
                name = enemy.name,
                message = message
            });
        }

        dialogRoutine = RunDialog();
        StartCoroutine(dialogRoutine);
    }

    public IEnumerator StartDialog()
    {
        dialogRoutine = RunDialog();
        yield return RunDialog();
    }
    
    private IEnumerator RunDialog()
    {
        while (StillHaveDialogsToShow())
        {
            nextRoutine = Next();
            yield return nextRoutine;
        }
    }

    private IEnumerator Next()
    {
        switch (currentState)
        {
            case DialogBoxState.Closing:
                CloseDialogInstantly();
                break;

            // If box is closed, open it 
            case DialogBoxState.Closed:
                ChangeDialogBoxState(DialogBoxState.Opening);
                openingRoutine = OpenDialogRoutine();
                StartCoroutine(openingRoutine);
                // This is used cause the user can advance the current action clicking on screen
                yield return new WaitUntil(() => userClickedOnScreen || currentState != DialogBoxState.Opening);
                StopCoroutine(openingRoutine);
                break;

            // If I click while the box is opening, it will instantly finish the routine
            case DialogBoxState.Opening:
                OpenDialogInstantly();
                break;

            // Waiting to show the next text
            case DialogBoxState.Waiting:
                currentMessageIndex++;

                if (currentMessageIndex >= dialogSpeakers.Count)
                {
                    yield return CloseDialogRoutine();

                    yield break;
                }

                ChangeDialogBoxState(DialogBoxState.Typing);
                writeRoutine = Write(dialogSpeakers[currentMessageIndex]);
                StartCoroutine(writeRoutine);
                yield return new WaitUntil(() => userClickedOnScreen || currentState != DialogBoxState.Typing);
                StopCoroutine(writeRoutine);
                userClickedOnScreen = false;
                yield return EndMessage(dialogSpeakers[currentMessageIndex].message);
                break;

            // It is typing, finish the type routine immediately
            case DialogBoxState.Typing:
                yield return EndMessage(dialogSpeakers[currentMessageIndex].message);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        userClickedOnScreen = false;
    }

    private IEnumerator Write (DialogContext speaker)
    {
        ChangeDialogBoxState(DialogBoxState.Typing);

        OnStartMessage?.Invoke();

        SetupDialog(speaker);

        var timeBetweenLetters = 1f / lettersPerSecond;
        textPanel.text = "";

        foreach (var character in speaker.message)
        {
            textPanel.text += character;

            yield return new WaitForSeconds(timeBetweenLetters);
        }

        yield return EndMessage(speaker.message);
    }

    private IEnumerator EndMessage (string message)
    {
        ChangeDialogBoxState(DialogBoxState.Waiting);
        textPanel.text = message;

        OnFinishMessage?.Invoke();

        // This will end by calling the AdvanceDialog method
        yield return new WaitUntil(() => userClickedOnScreen);
    }

    private bool StillHaveDialogsToShow()
        => dialogSpeakers.Count != 0;
}