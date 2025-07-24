using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class LogMessage : MonoBehaviour
{
    [Title("References")]
    [SerializeField]
    private TMP_Text titleLabel;

    [SerializeField]
    private TMP_Text descriptionLabel;

    public void Setup (string title, string description)
    {
        titleLabel.text = title;
        descriptionLabel.text = description;
    }
}