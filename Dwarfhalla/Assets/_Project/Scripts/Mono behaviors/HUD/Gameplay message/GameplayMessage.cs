using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameplayMessage : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text mainText;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text subText;

    private void Awake()
    {
        ServiceLocator.GameplayMessage = this;

        GameEntryPoints.OnLoadAssets += _ => mainText.text = "Loading assets...";
        GameEntryPoints.OnGeneratedPlayerData += _ => mainText.text = "Generating player data...";
        GameEntryPoints.OnRenderingLevel += _ => mainText.text = "Rendering level...";
    }

    public void UpdateMainMessage (string m)
    {
        mainText.text = m;
        UpdateSubMessage("");
    }

    public void UpdateSubMessage (string m) => subText.text = m;
}