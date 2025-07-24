using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New job", menuName = EditorConstants.MenuAssets.GAME_JOB + "New job")]
public partial class GameJob : ScriptableObject, IPotentialProvider
{
    [field: TitleGroup("Settings")]
    [field: HorizontalGroup("Settings/horizontal")]
    [field: SerializeField]
    public string Name { get; private set; }

    [field: TitleGroup("Settings")]
    [field: HorizontalGroup("Settings/horizontal")]
    [field: HideLabel]
    [field: PreviewField]
    [field: SerializeField]
    public Sprite JobIcon { get; private set; }

    [field: TitleGroup("Settings")]
    [field: HideLabel]
    [field: Multiline(5)]
    [field: SerializeField]
    public string Description { get; private set; }

    [field: BoxGroup("Custom behaviors")]
    [field: SerializeField]
    public JobSkill SpecialSkill { get; private set; }

    [field: BoxGroup("Custom behaviors")]
    [field: SerializeField]
    public SpecialResource SpecialResource { get; private set; }

    [field: BoxGroup("Attributes bonus")]
    [field: HideLabel]
    [field: SerializeField]
    public PotentialBonus PotentialBonus { get; set; }

    [BoxGroup("Requisites")]
    [TitleGroup("Requisites/Level requisite")]
    [MinMaxSlider(1, GameConstants.LEVEL_CAP, true)]
    [SerializeField]
    private Vector2Int level = new(5, 5);

    [TitleGroup("Requisites/Trait requisites")]
    [HideLabel]
    [SerializeField]
    private TraitRequisites traitRequisites;

    [TitleGroup("Debug")]
    [HideInEditorMode]
    [ShowInInspector]
    private SpecialResource instanceSpecialResource;

    public virtual IEnumerator LoadInstances()
    {
        instanceSpecialResource = SpecialResource.GetInstance;
        yield break;
    }

    public bool CanBecomeThisClass (PlayableCharacterData characterData)
    {
        if (!Vector2IntHelper.IsInRange(level, characterData.CurrentLevel))
            return false;

        if (!traitRequisites.Validate(characterData))
            return false;

        return true;
    }
}

[Serializable]
public class PotentialBonus
{
    [field:
        ProgressBar(0, GameConstants.MAX_JOB_SECTOR_ATTRIBUTE_BONUS, 1, 0, 0, Segmented = true, DrawValueLabel = true)]
    [field: SerializeField]
    public int Strength { get; private set; }

    [field:
        ProgressBar(0, GameConstants.MAX_JOB_SECTOR_ATTRIBUTE_BONUS, 0, 1, 0, Segmented = true, DrawValueLabel = true)]
    [field: SerializeField]
    public int Dexterity { get; private set; }

    [field:
        ProgressBar(0, GameConstants.MAX_JOB_SECTOR_ATTRIBUTE_BONUS, 1, 1, 0, Segmented = true, DrawValueLabel = true)]
    [field: SerializeField]
    public int Vitality { get; private set; }

    [field:
        ProgressBar(0, GameConstants.MAX_JOB_SECTOR_ATTRIBUTE_BONUS, 0, 1, 1, Segmented = true, DrawValueLabel = true)]
    [field: SerializeField]
    public int Intelligence { get; private set; }
}

[Serializable]
public class TraitRequisites
{
    [SerializeField]
    private bool hasStrengthRequisite;

    [MinMaxSlider(3, GameConstants.MAX_COMBINED_ATTRIBUTE_AMOUNT, true)]
    [EnableIf("@hasStrengthRequisite")]
    [SerializeField]
    private Vector2Int strength;

    [SerializeField]
    private bool hasDexterityRequisite;

    [MinMaxSlider(3, GameConstants.MAX_COMBINED_ATTRIBUTE_AMOUNT, true)]
    [EnableIf("@hasDexterityRequisite")]
    [SerializeField]
    private Vector2Int dexterity;

    [SerializeField]
    private bool hasVitalityRequisite;

    [MinMaxSlider(3, GameConstants.MAX_COMBINED_ATTRIBUTE_AMOUNT, true)]
    [EnableIf("@hasVitalityRequisite")]
    [SerializeField]
    private Vector2Int vitality;

    [SerializeField]
    private bool hasIntelligenceRequisite;

    [MinMaxSlider(3, GameConstants.MAX_COMBINED_ATTRIBUTE_AMOUNT, true)]
    [EnableIf("@hasIntelligenceRequisite")]
    [SerializeField]
    private Vector2Int intelligence;

    public bool Validate (PlayableCharacterData characterData)
    {
        if (hasStrengthRequisite
            && !Vector2IntHelper.IsInRange(strength, characterData.TraitChart.StrengthSector.SectorPotential))
            return false;

        if (hasDexterityRequisite
            && !Vector2IntHelper.IsInRange(dexterity, characterData.TraitChart.DexteritySector.SectorPotential))
            return false;

        if (hasVitalityRequisite
            && !Vector2IntHelper.IsInRange(vitality, characterData.TraitChart.VitalitySector.SectorPotential))
            return false;

        if (hasIntelligenceRequisite
            && !Vector2IntHelper.IsInRange(intelligence, characterData.TraitChart.IntelligenceSector.SectorPotential))
            return false;

        return true;
    }
}