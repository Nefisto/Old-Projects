using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MagickaResourceHUD : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [Title("HUD references")]
    [SerializeField]
    private TMP_Text redLabel;

    [SerializeField]
    private TMP_Text blackLabel;

    [SerializeField]
    private TMP_Text whiteLabel;

    private void OnEnable()
        => player.OnUpdatedMagickaResource += UpdateMagickaResourceOnHUD;

    private void OnDisable()
        => player.OnUpdatedMagickaResource -= UpdateMagickaResourceOnHUD;

    private void UpdateMagickaResourceOnHUD (MagickaResource magickaResource)
    {
        redLabel.text = $"{magickaResource.red}";
        blackLabel.text = $"{magickaResource.black}";
        whiteLabel.text = $"{magickaResource.white}";
    }
}