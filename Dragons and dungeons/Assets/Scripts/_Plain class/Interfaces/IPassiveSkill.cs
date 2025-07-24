using System.Collections.Generic;

public interface IPassiveSkill
{
    IEnumerable<SkillPassive> GetValidPassiveSkill (SkillValidatorContext ctx);
}