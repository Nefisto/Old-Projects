using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public partial class CoinAnimation : MonoBehaviour, IAnimation
{
    [TitleGroup("Settings")]
    [Range(0.1f, 2f)]
    [SerializeField]
    private float growingDuration = 1f;

    [TitleGroup("Settings")]
    [Range(0.1f, 3f)]
    [SerializeField]
    private float timeForAFullRotation = 1f;

    [TitleGroup("References")]
    [SerializeField]
    private Transform modelFolder;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text amountLabel;

    public void Setup()
    {
        modelFolder.localScale = Vector3.zero;
        amountLabel.text = "";
    }
}