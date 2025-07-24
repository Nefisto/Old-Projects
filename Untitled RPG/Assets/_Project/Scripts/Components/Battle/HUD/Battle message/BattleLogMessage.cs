using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BattleLogMessage : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text message;

    [DisableInEditorButton]
    public void Setup (string message) => this.message.text = message;
}