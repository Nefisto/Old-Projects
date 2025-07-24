using UnityEngine;

public abstract class SkillValidator : ScriptableObject
{
    public abstract bool Validate(SkillValidatorContext validatorContext);
}