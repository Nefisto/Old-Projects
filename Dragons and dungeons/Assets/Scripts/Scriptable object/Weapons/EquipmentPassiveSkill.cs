using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class EquipmentPassiveSkill
{
    public SkillPassive skill;
    public List<SkillValidator> validator;

    public bool CanUseSkill(SkillValidatorContext ctx)
        => validator
            .All(sv => sv.Validate(ctx));    
}