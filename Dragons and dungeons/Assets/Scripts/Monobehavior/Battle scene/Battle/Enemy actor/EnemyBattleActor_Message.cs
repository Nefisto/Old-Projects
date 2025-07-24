using System.Collections;
using NTools;

public partial class EnemyBattleActor
{
    private DialogBox dialogBox;

    #region Temporary - To show dialog only - Unity button does not accept custom parameters... am I dont want to bind a temp button here

    public IEnumerator ShowBeginningBattleMessage()
    {
        AddMessagesToDialog(DialogCommonEvents.Start);

        yield return dialogBox.StartDialog();
    }
    
    public void ShowDieBattleMessage ()
    {
        AddMessagesToDialog(DialogCommonEvents.Death);
    }

    public void ShowCustomEventMessage (string customEventID)
    {
        AddMessagesToDialog(DialogCommonEvents.Custom, customEventID);
    }

    #endregion

    private void AddMessagesToDialog (DialogCommonEvents commonEvents, string customEvent = null)
    {
        var randomizedDialog = EnemyData
            .GetDialogs(commonEvents, customEvent)
            .NTGetRandom();

        foreach (var message in randomizedDialog)
        {
            dialogBox.AddDialog(new DialogContext
            {
                icon = EnemyData.icon,
                bigSprite = randomizedDialog.BigSprite,
                name = EnemyData.name,
                life = $"{CurrentLife} / {Data.GetBaseStatus().MaxHealth}",
                message = message
            });
        }
    }

    private void CacheDialogBox()
        => dialogBox = FindObjectOfType<DialogBox>();
}