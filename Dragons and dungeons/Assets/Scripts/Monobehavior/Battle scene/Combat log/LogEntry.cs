using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class LogEntry : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private TextMeshProUGUI message;

    public void Setup(CombatLogEntry entry)
    {
        message.text = entry.Message;
    }
}