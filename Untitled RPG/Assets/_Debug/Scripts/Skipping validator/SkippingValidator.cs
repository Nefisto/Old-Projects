using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
public class SkippingValidator : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private TemplateDataFactory templateToUse;

    private void Awake()
    {
        GameEvents.onGameStart += SkipCharacterCreationValidator;
    }

    private void SkipCharacterCreationValidator()
    {
        if (!Database.GameConstantsSo.shouldSkipCharacterCreationView)
            return;

        var characterCreation = FindObjectOfType<CharacterCreation2>();
        characterCreation.SkipCharacterCreation(templateToUse != null ? templateToUse.GetInstance() : null);
    }
}
#endif