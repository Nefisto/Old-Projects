using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.StatusValidatorName, menuName = Nomenclature.StatusValidatorMenu, order = 0)]
public class StatusValidator : SkillValidator
{
    [Title("Requirements")]
    [HideLabel]
    public RequirementsStatus requirement;

    public override bool Validate (SkillValidatorContext validatorContext)
        => validatorContext.status >= requirement;
}