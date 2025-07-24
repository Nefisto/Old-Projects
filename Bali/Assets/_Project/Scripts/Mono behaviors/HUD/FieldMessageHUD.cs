using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FieldMessageHUD : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField]
    private Player playerToSync;

    [SerializeField]
    private TMP_Text messageLabel;

    private void OnEnable()
        => EventHandler.RegisterEvent<string>(playerToSync, GameEvents.CHANGE_FIELD_MESSAGE, RefreshMessage);

    private void OnDisable()
        => EventHandler.UnregisterEvent<string>(playerToSync, GameEvents.CHANGE_FIELD_MESSAGE, RefreshMessage);

    private void RefreshMessage (string newMessage)
        => messageLabel.text = newMessage;
}