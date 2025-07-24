using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillDetailedInfo : MonoBehaviour, IMenu
{
    [TitleGroup("References")]
    [SerializeField]
    private Image skillIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text skillName;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text skillDescription;

    [TitleGroup("References")]
    [SerializeField]
    private Transform attributeInfoFolder;

    [FormerlySerializedAs("attributeEntryPrefab")]
    [TitleGroup("References")]
    [SerializeField]
    private AttributeInfoEntry2 attributeEntry2Prefab;

    private void Awake() => GameEvents.OpenDetailedSkillInfoEntryPoint += Setup;

    private IEnumerator Setup (Settings settings)
    {
        var skill = settings.skill;

        skillIcon.sprite = skill.Icon;
        skillName.text = skill.Name;
        skillDescription.text = skill.Description;

        ClearOldAttributes();
        foreach (var attributeOnHUD in skill.AttributesIterator())
        {
            var instance = Instantiate(attributeEntry2Prefab, attributeInfoFolder, false);

            yield return instance.Setup(attributeOnHUD);
        }

        ServiceLocator.MenuStack.OpenMenu(this);
    }

    private void ClearOldAttributes()
    {
        foreach (Transform child in attributeInfoFolder)
            Destroy(child.gameObject);
    }

    public class Settings
    {
        public Skill skill;
    }
}