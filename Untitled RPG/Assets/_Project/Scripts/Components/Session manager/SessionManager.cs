using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class SessionManager : MonoBehaviour
{
    [field: TitleGroup("Debug")]
    [field: SerializeField]
    public PlayableCharacterData PlayableCharacterData { get; private set; }

    [field: TitleGroup("Debug")]
    [field: ReadOnly]
    [field: SerializeField]
    public Template LastUsedTemplate { get; private set; }

    [TitleGroup("Debug")]
    [ShowInInspector]
    private string LastUsedTemplateSavePath
        => Application.persistentDataPath + $"/{GameConstants.SAVE_FILE_NAME}_Last_used.json";

    protected void Start()
    {
        GameEvents.OnGameOverRetryButtonPressed += RetryWithSameTemplateListener;
        GameEvents.OnBeginningAdventure += BeginAdventureListener;
    }

    private void RetryWithSameTemplateListener()
    {
        LastUsedTemplate ??= Helper.LoadTemplate(LastUsedTemplateSavePath);

        InternalBeginAdventure(LastUsedTemplate);
    }

    private void BeginAdventureListener (IEntryPointContext ctx)
    {
        var template = ctx as Template;
        LastUsedTemplate = template.GetInstance;
        Helper.SaveTemplate(LastUsedTemplateSavePath, LastUsedTemplate);

        InternalBeginAdventure(template);
    }

    private void InternalBeginAdventure (Template template)
    {
        StartCoroutine(Behavior());

        IEnumerator Behavior()
        {
            yield return ServiceLocator.ScreenFading.FadeIn();
            yield return SetupFromTemplate(template);
            yield return GameEvents.OnSetupAdventure?.YieldableInvoke();
            yield return ServiceLocator.ScreenFading.FadeOut();
            yield return GameEvents.OnBegunAdventureEntryPoint.YieldableInvoke();
        }
    }

    private IEnumerator SetupFromTemplate (Template template)
    {
        var data = new PlayableCharacterData();
        yield return data.Setup(new PlayableCharacterData.Context
        {
            equipment = template.currentEquipment,
            traitChart = template.traitChart
        });

        PlayableCharacterData = data;
        OnLoadedPlayerCharacterData?.Invoke(PlayableCharacterData);
    }


    public static event Action<PlayableCharacterData> OnLoadedPlayerCharacterData;
}