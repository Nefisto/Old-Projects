using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[SelectionBase]
public partial class StatusMenuView : MonoBehaviour, IMenu
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text levelLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text currencyAmount;

    [TitleGroup("References")]
    [SerializeField]
    private AttributeChartView attributeChartView;

    [TitleGroup("References")]
    [SerializeField]
    private PotentialView potentialView;

    [TitleGroup("References")]
    [SerializeField]
    private EquipmentSlot2 weaponSlot;

    [TitleGroup("References")]
    [SerializeField]
    private EquipmentSlot2 armorSlot;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJob job;

    public IEnumerator Setup (PlayableCharacterData characterData)
    {
        levelLabel.text = $"Level: {characterData.CurrentLevel:000}";
        currencyAmount.text = $"{characterData.CurrentCurrency}";

        yield return SetupWeaponSlot(characterData);
        yield return SetupArmorSlot(characterData);

        yield return attributeChartView.Setup(characterData.TraitChart);
        yield return potentialView.Setup(characterData);
        yield return job.Setup(characterData);
    }

    private IEnumerator SetupWeaponSlot (PlayableCharacterData characterData)
    {
        yield return weaponSlot.Setup(new EquipmentSlot2.Settings()
        {
            equipmentData = characterData.CurrentEquipment.CurrentWeapon
        });
    }

    private IEnumerator SetupArmorSlot (PlayableCharacterData characterData)
    {
        yield return armorSlot.Setup(new EquipmentSlot2.Settings()
        {
            equipmentData = characterData.CurrentEquipment.CurrentArmor
        });
    }
}