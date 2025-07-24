using System.Collections.Generic;

public interface IActiveSkill
{
    IEnumerable<Skill> GetValidActiveSkill (SkillValidatorContext ctx);
}