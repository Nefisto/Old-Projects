using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobResourceDetail : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image featIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text featName;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text featDescription;

    public IEnumerator Setup (SpecialResource specialResource)
    {
        // featIcon.sprite = specialResource.icon;
        // featName.text = settings.name;
        // featDescription.text = settings.description;
        yield break;
    }

    public class Settings
    {
        public string description;
        public Sprite icon;
        public string name;
    }
}