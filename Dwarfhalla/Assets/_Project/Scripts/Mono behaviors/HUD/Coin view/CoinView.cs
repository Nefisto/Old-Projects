using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CoinView : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text coinLabel;

    private void Awake()
    {
        GameEntryPoints.OnGeneratedPlayerData += _ => Setup(ServiceLocator.GameContext.PlayerData);
    }

    private void Setup (PlayerData playerData)
    {
        coinLabel.text = "0";
        playerData.Coins.OnValueChanged += UpdateCoinHUD;
    }

    private void UpdateCoinHUD (int old, int current)
    {
        coinLabel.text = current.ToString();
    }
}