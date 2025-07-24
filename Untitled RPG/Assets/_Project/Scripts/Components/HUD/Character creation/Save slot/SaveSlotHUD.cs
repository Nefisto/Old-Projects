using System;
using System.Collections;
using System.IO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public partial class SaveSlotHUD : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private SaveSlotNumber slotNumber;

    [TitleGroup("References")]
    [SerializeField]
    private Image characterIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text strengthPotentialLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text dexterityPotentialLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text vitalityPotentialLabel;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text intelligencePotentialLabel;

    [TitleGroup("References")]
    [SerializeField]
    private Image weaponEquipped;

    [TitleGroup("References")]
    [SerializeField]
    private Image armorEquipped;

    [FormerlySerializedAs("selectButton")]
    [TitleGroup("References")]
    [SerializeField]
    private Button loadButton;

    [TitleGroup("References")]
    [SerializeField]
    private Button saveButton;

    [TitleGroup("References")]
    [SerializeField]
    private Button deleteButton;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private Template currentTemplate;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private string SavePath
        => Application.persistentDataPath + $"/{GameConstants.SAVE_FILE_NAME}_{slotNumber.ToString()}.json";

    public IEnumerator Setup (Context context)
    {
        SetupSaveOperation(context.saveOperation);
        SetupLoadOperation(context.loadOperation);
        SetupDeleteButton();
        yield return LoadSlot();
    }

    private void SetupDeleteButton()
    {
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(() => StartCoroutine(EraseSlot()));
    }

    private void SetupLoadOperation (Action<Template> loadOperation)
    {
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(() => loadOperation?.Invoke(currentTemplate));
    }

    private void SetupSaveOperation (Action saveOperation)
    {
        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(saveOperation.Invoke);
    }

    private IEnumerator LoadSlot()
    {
        if (!File.Exists(SavePath))
        {
            yield return SetupEmptySlot();
            yield break;
        }

        var data = File.ReadAllText(SavePath);
        currentTemplate = JsonUtility.FromJson<Template>(data);
        currentTemplate.LoadReferences();

        yield return SetupSlot(currentTemplate);
    }

    public IEnumerator SaveSlot (Template template)
    {
        currentTemplate = template;
        yield return SetupSlot(template);
        yield return Save(template);
    }

    private IEnumerator EraseSlot()
    {
        yield return SetupEmptySlot();
        yield return EraseSave();
    }

    private IEnumerator SetupSlot (Template template)
    {
        yield return EnableInformation();

        yield return SetupCharacterIcon(template.characterIconEnum);
        yield return SetupPotential(template.traitChart);
        yield return SetupGear(template.currentEquipment);

        loadButton.interactable = true;
        saveButton.interactable = true;
        deleteButton.interactable = true;
    }

    private IEnumerator SetupEmptySlot()
    {
        yield return DisableInformation();
        characterIcon.sprite =
            Database.GameIcons.CharacterCreationIcons.GetIconOfKind(CharacterCreationIcons.EmptySlot);

        loadButton.interactable = false;
        saveButton.interactable = true;
        deleteButton.interactable = false;
    }

    private IEnumerator Save (Template template)
    {
        var data = JsonUtility.ToJson(template, true);
        File.WriteAllText(SavePath, data);
        yield break;
    }

    private IEnumerator SetupCharacterIcon (SaveSlotIconEnum iconKey)
    {
        characterIcon.sprite = Database.GameIcons.SaveSlotIcons.GetIconOfKind(iconKey);
        yield break;
    }

    private IEnumerator SetupGear (CurrentEquipment currentEquipment)
    {
        weaponEquipped.sprite = currentEquipment.CurrentWeapon.Icon;
        armorEquipped.sprite = currentEquipment.CurrentArmor.Icon;
        yield break;
    }

    private IEnumerator SetupPotential (TraitChart traitChart)
    {
        strengthPotentialLabel.text = traitChart.StrengthPotential.ToString();
        dexterityPotentialLabel.text = traitChart.DexterityPotential.ToString();
        vitalityPotentialLabel.text = traitChart.VitalityPotential.ToString();
        intelligencePotentialLabel.text = traitChart.IntelligencePotential.ToString();
        yield break;
    }

    private IEnumerator EraseSave()
    {
        File.Delete(SavePath);
        yield break;
    }

    private IEnumerator EnableInformation()
    {
        strengthPotentialLabel.gameObject.SetActive(true);
        dexterityPotentialLabel.gameObject.SetActive(true);
        vitalityPotentialLabel.gameObject.SetActive(true);
        intelligencePotentialLabel.gameObject.SetActive(true);
        weaponEquipped.gameObject.SetActive(true);
        armorEquipped.gameObject.SetActive(true);
        yield break;
    }

    private IEnumerator DisableInformation()
    {
        strengthPotentialLabel.gameObject.SetActive(false);
        dexterityPotentialLabel.gameObject.SetActive(false);
        vitalityPotentialLabel.gameObject.SetActive(false);
        intelligencePotentialLabel.gameObject.SetActive(false);
        weaponEquipped.gameObject.SetActive(false);
        armorEquipped.gameObject.SetActive(false);
        yield break;
    }

    public class Context
    {
        public Action<Template> loadOperation;
        public Action saveOperation;
    }

    private enum SaveSlotNumber
    {
        One,
        Two,
        Three
    }
}