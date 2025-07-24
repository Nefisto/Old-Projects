using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class JobInfoView : MonoBehaviour, IMenu
{
    [TitleGroup("References")]
    [SerializeField]
    private Image jobIcon;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text jobName;

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text jobDescription;

    [TitleGroup("References")]
    [SerializeField]
    private JobSkillDetail jobSkill;

    [TitleGroup("References")]
    [SerializeField]
    private JobResourceDetail jobResource;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector strengthBonus;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector dexterityBonus;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector vitalityBonus;

    [TitleGroup("References")]
    [SerializeField]
    private StatusViewJobBonusSector intelligenceBonus;

    private void Awake() => GameEvents.OpenGameJobViewEntryPoint += OpenMenuListener;

    private IEnumerator OpenMenuListener (GameJob job)
    {
        yield return Setup(job);
        ServiceLocator.MenuStack.OpenMenu(this);
    }

    private IEnumerator Setup (GameJob job)
    {
        jobIcon.sprite = job.JobIcon;
        jobName.text = job.Name;
        jobDescription.text = job.Description;

        yield return jobSkill.Setup(job.SpecialSkill);

        yield return strengthBonus.Setup(job.StrengthPotential);
        yield return dexterityBonus.Setup(job.DexterityPotential);
        yield return vitalityBonus.Setup(job.VitalityPotential);
        yield return intelligenceBonus.Setup(job.IntelligencePotential);
    }
}