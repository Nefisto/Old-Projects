using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private SkillButton defaultAttackButton;

    [TitleGroup("References")]
    [SerializeField]
    private SkillButton defaultDefenseButton;

    [TitleGroup("References")]
    [SerializeField]
    private SkillButton specialSkill;

    [field: TitleGroup("References")]
    [field: SerializeField]
    private GradientBar specialResource;

    [TitleGroup("References")]
    [SerializeField]
    private List<SkillButton> skillButtons;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    public NDictionary<SkillButton, Skill> ButtonToSkill { get; private set; } = new();

    private void Awake()
    {
        ServiceLocator.BattleUI = this;
        GameEvents.onSelectSkill += UpdateSelectedSkill;
        GameEvents.onChangedSkill += skill =>
        {
            if (skill.ShouldResetTarget)
                ServiceLocator.TargetSelector.Clear();
        };
    }

    public IEnumerator Setup (SetupSettings settings)
    {
        ButtonToSkill = skillButtons
            .Zip(settings.skills,
                (button, skill) => (button, skill))
            .ToNDictionary(t => t.button, t => t.skill);
        ButtonToSkill.Add(defaultAttackButton, settings.defaultAttack);
        ButtonToSkill.Add(defaultDefenseButton, settings.defaultDefense);

        foreach (var (button, skill) in ButtonToSkill)
        {
            button.Setup(new SkillButton.SetupSettings
            {
                skill = skill,
                onPressedCallback = _ => GameEvents.onSelectSkill?.Invoke(skill),
                usedResource = settings.casterManaResource
            });
        }

        SetupSpecialSkill(settings);

        yield return settings.job.SpecialResource.Setup(new SpecialResource.SetupSettings
        {
            gradientBar = specialResource
        });
    }

    private void SetupSpecialSkill (SetupSettings settings)
    {
        if (settings.job.SpecialSkill == null)
            specialSkill.gameObject.SetActive(false);
        else
        {
            specialSkill.gameObject.SetActive(true);
            ButtonToSkill.Add(specialSkill, settings.job.SpecialSkill);

            specialSkill.Setup(new SkillButton.SetupSettings
            {
                usedResource = settings.job.SpecialResource is NoResource
                    ? settings.casterManaResource
                    : settings.job.SpecialResource,
                skill = settings.job.SpecialSkill,
                onPressedCallback = _ => GameEvents.onSelectSkill?.Invoke(settings.job.SpecialSkill),
            });
        }
    }

    private void UpdateSelectedSkill (Skill skill)
    {
        UnselectAllButtons();

        ButtonToSkill
            .Where(pair => pair.Value == skill)
            .Select(p => p.Key)
            .ForEach(sb => sb.SetSkillSelected(true));
    }

    private void UnselectAllButtons()
    {
        foreach (var skillButton in ButtonToSkill.Keys)
            skillButton.SetSkillSelected(false);
    }

    public class SetupSettings
    {
        public ManaResource casterManaResource;
        public Skill defaultAttack;
        public Skill defaultDefense;
        public GameJob job;
        public IEnumerable<Skill> skills;
    }
}