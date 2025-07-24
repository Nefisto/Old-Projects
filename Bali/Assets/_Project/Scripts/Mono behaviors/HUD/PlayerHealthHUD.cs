using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerHealthHUD : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField]
    private Player player;

    [SerializeField]
    private TMP_Text lifeLabel;

    private void OnEnable()
        => player.OnUpdatedLife += RefreshHealthLabel;

    private void OnDisable()
        => player.OnUpdatedLife -= RefreshHealthLabel;

    private void RefreshHealthLabel (int newLife)
        => lifeLabel.text = $"{newLife}";
}