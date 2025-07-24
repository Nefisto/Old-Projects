using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

// HOLY SHIT, IF I REMOVE THE NUMBER THE SCRIPT GET LOST ON INSPECTOR...
public partial class CharacterCreation2 : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image saveSlotIcon;

    [TitleGroup("References")]
    [SerializeField]
    private SlotIcon slotIcon;

    [TitleGroup("References")]
    [SerializeField]
    private SlotIconGrid slotIconGrid;

    [TitleGroup("References")]
    [SerializeField]
    private Button beginButton;

    [TitleGroup("References")]
    [SerializeField]
    private TraitBuildHUD traitBuildHUD;

    [TitleGroup("References")]
    [SerializeField]
    private EquippedGearOnCreation currentEquipment;

    [TitleGroup("References")]
    [SerializeField]
    private List<SaveSlotHUD> saveSlots;

    [TitleGroup("References")]
    [SerializeField]
    private GameObject hudFolder;

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private Template usedTemplate;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private Template cachedTemplate;

    [TitleGroup("Debug")]
    [ShowInInspector]
    private string TemporarySavePath => Application.persistentDataPath + $"/{GameConstants.SAVE_FILE_NAME}_Temp.json";

    private void Awake()
    {
        GameLoader.ThingsToLoadEntryPoint += () => Setup(null);

        GameEvents.OpenCharacterCreationMenu += () =>
        {
            hudFolder.SetActive(true);
            StartCoroutine(OpenMenuListener());
        };
    }

    private void OnApplicationQuit() => Helper.SaveTemplate(TemporarySavePath, usedTemplate);

    private IEnumerator Setup (Template toUseTemplate)
    {
        UpdateTemplate(toUseTemplate);

        usedTemplate.Setup();
        yield return currentEquipment.Setup(usedTemplate.currentEquipment);
        yield return UpdateView(usedTemplate);
        yield return SetupSlotIcon();
        yield return SetupBeginAdventureButton();
        yield return SetupSaveSlots();
    }

    private IEnumerator UpdateView (Template template)
    {
        UpdateTemplate(template);

        currentEquipment.RefreshGear(usedTemplate.currentEquipment);
        yield return traitBuildHUD.Setup(usedTemplate.traitChart);
        yield return RefreshSaveIcon(usedTemplate);
    }

    private IEnumerator RefreshSaveIcon (Template template)
    {
        saveSlotIcon.sprite = Database.GameIcons.SaveSlotIcons.GetIconOfKind(template.characterIconEnum);
        yield break;
    }

    private IEnumerator SetupSlotIcon()
    {
        yield return slotIcon.Setup();
        yield return slotIconGrid.Setup();
        slotIconGrid.OnClick += ctx =>
        {
            if (!ctx.isUnlocked)
            {
                // Play FAIL SOUND
                return;
            }

            usedTemplate.characterIconEnum = ctx.iconEnum;
            saveSlotIcon.sprite = ctx.icon;
            ServiceLocator.MenuStack.CloseMenu();
        };
    }

    private IEnumerator SetupBeginAdventureButton()
    {
        beginButton.onClick.RemoveAllListeners();
        beginButton.onClick.AddListener(BeginAdventureListener);
        yield break;
    }

    private void UpdateTemplate (Template toUseTemplate)
    {
        usedTemplate = toUseTemplate ?? Helper.LoadTemplate(TemporarySavePath);
        cachedTemplate = usedTemplate.GetInstance;
    }

    private IEnumerator SetupSaveSlots()
    {
        foreach (var saveSlotHUD in saveSlots)
            yield return saveSlotHUD.Setup(new SaveSlotHUD.Context
            {
                saveOperation = () =>
                {
                    cachedTemplate = usedTemplate;
                    StartCoroutine(saveSlotHUD.SaveSlot(usedTemplate));
                },
                loadOperation = template =>
                {
                    var instanceTemplate = template.GetInstance;
                    var correctRoutine = usedTemplate.Equals(cachedTemplate)
                        ? UpdateView(instanceTemplate)
                        : QuestionBeforeUpdateView(instanceTemplate);

                    StartCoroutine(correctRoutine);
                }
            });
    }

    private IEnumerator QuestionBeforeUpdateView (Template template)
    {
        var questionBox = ServiceLocator.QuestionBox;
        bool? answer = null;
        yield return questionBox.Setup(new QuestionBox.Context()
        {
            label = "Your changes will be lost. Are you sure?",
            cancelOperation = () => answer = false,
            confirmOperation = () => answer = true
        });
        ServiceLocator.MenuStack.OpenMenu(questionBox);

        yield return new WaitUntil(() => answer != null);

        if (answer == false)
            yield break;

        yield return UpdateView(template);
    }

    private void DisableView() => hudFolder.SetActive(false);

    private IEnumerator OpenMenuListener()
    {
        yield return ServiceLocator.ScreenFading.FadeIn();
        yield return Setup(null);
        yield return ServiceLocator.ScreenFading.FadeOut();
    }

    private void BeginAdventureListener()
    {
        StartCoroutine(Behavior());

        IEnumerator Behavior()
        {
            yield return ServiceLocator.ScreenFading.FadeIn();
            Helper.SaveTemplate(TemporarySavePath, usedTemplate);
            DisableView();
            yield return GameEvents.OnBeginningAdventure.YieldableInvoke(usedTemplate);
        }
    }
}