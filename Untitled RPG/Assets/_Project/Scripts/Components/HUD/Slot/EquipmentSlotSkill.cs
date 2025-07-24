using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotSkill : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image skillIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text skillName;

    public void Setup (Skill skill)
    {
        if (skill == null)
        {
            SetupEmptySkill();
            return;
        }

        skillIcon.enabled = true;

        skillIcon.sprite = skill.Icon;
        skillName.text = $"{skill.name}";
    }

    private void SetupEmptySkill()
    {
        skillIcon.enabled = false;

        skillName.text = "---";
    }
}