using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debug_SyncSlider : MonoBehaviour
{
    public Slider slider;

    public TMP_Text textToSync;

    private void Start()
    {
        slider.onValueChanged.AddListener(value => textToSync.text = $"{value:F2}");
        textToSync.text = $"{slider.value:F2}";
    }
}