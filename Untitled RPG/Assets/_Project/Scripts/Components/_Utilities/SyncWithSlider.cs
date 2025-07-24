using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class SyncWithSlider : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Slider slider;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
        slider.onValueChanged.AddListener(UpdateLabel);
    }

    private void Start() => UpdateLabel(slider.value);

    private void UpdateLabel (float value) => label.text = $"{value:F0}";
}