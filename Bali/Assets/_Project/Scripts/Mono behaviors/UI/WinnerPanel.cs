using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class WinnerPanel : MonoBehaviour
{
    [Title("HUD references")]
    [SerializeField]
    private TMP_Text winnerMessageLabel;
    
    public void Show(string winnerMessage)
    {
        gameObject.SetActive(true);
        winnerMessageLabel.text = winnerMessage;
    }

    public void HidePanel()
        => gameObject.SetActive(false);
}