using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class AttributeInfoOnPauseEntry : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text label;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text value;

    public void Setup (string attributeName, int attributeValue)
    {
        label.text = attributeName;
        value.text = $"{attributeValue}";
    }
}