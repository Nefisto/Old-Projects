using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

// TODO: Again, if I remove the 2 their connection wiht GO simple get lost
public class AttributeInfoEntry2 : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text attributeName;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text attributeValue;

    public IEnumerator Setup (Skill.AttributeOnHUD attributeOnHUD)
    {
        attributeName.text = attributeOnHUD.label;
        attributeValue.text = attributeOnHUD.value;
        yield break;
    }
}