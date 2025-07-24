using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoCardContext
{
    public string Title;
    public Sprite Icon;
    public string Description;
}

public class InfoCard : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private TextMeshProUGUI tmpName;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI tmpDescription;
    
    public void ShowInfoCard(InfoCardContext ctx)
    {
        tmpName.text = ctx.Title;
        icon.sprite = ctx.Icon;
        tmpDescription.text = ctx.Description;
    }
}