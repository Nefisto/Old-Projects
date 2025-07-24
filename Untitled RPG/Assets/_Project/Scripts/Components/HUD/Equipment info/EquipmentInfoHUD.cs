using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
[RequireComponent(
    typeof(ResetAnchoredPositionOnStart),
    typeof(DisableMenuOnStart))]
public partial class EquipmentInfoHUD : MonoBehaviour, IMenu
{
    [TitleGroup("References")]
    [SerializeField]
    private Image equipmentIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text equipmentName;

    [TitleGroup("References")]
    [SerializeField]
    private SkillInfoSimple defaultSkillInfo;

    [TitleGroup("References")]
    [SerializeField]
    private SkillInfoSimple skillAInfo;

    [TitleGroup("References")]
    [SerializeField]
    private SkillInfoSimple skillBInfo;

    private void Awake()
    {
        GameEvents.OnOpenEquipmentInfo += data =>
        {
            _ = new NTask(Setup(data));
            ServiceLocator.MenuStack.OpenMenu(this);
        };
    }

    [TitleGroup("Debug")]
    [DisableInEditorButton]
    private IEnumerator Setup (EquipmentData equipment)
    {
        equipmentIcon.sprite = equipment.Icon;
        equipmentName.text = equipment.Name;

        yield return defaultSkillInfo.Setup(equipment.DefaultSkill);
        yield return skillAInfo.Setup(equipment.SkillA);
        yield return skillBInfo.Setup(equipment.SkillB);
    }
}