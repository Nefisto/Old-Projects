using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeEquipmentEntry : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Image icon;

    [TitleGroup("Settings")]
    [SerializeField]
    private TMP_Text label;

    public IEnumerator Setup (EquipmentData equipmentData)
    {
        icon.sprite = equipmentData.Icon;
        label.name = equipmentData.Name;

        yield return null;
    }
}