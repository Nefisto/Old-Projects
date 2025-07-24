using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EquipmentData : ItemData, IActiveSkill, IPassiveSkill
{
    [TitleGroup("Graphics")]
    [SerializeField]
    private bool customRuntimeImage = false;

    [TitleGroup("Graphics")]
    [ShowIf(@"customRuntimeImage")]
    [PreviewField]
    [SerializeField]
    private Sprite runtimeImage;

    [TitleGroup("Graphics")]
    [Tooltip("Changing this will make system look for a specific child inside character prefab equipment.")]
    [Min(0)]
    public int idOnRuntime = 0;
    
    [TabGroup("Skills")]
    [Title("Active")]
    [SerializeField]
    private List<EquipmentActiveSkill> activeSkills;

    public override Sprite GetIconSprite => customRuntimeImage 
        ? runtimeImage : icon;

    [field: SerializeField]
    public override bool CanStack { get; protected set; } = false;

    protected override ItemData CreateNewInstance { get; }

    [Space]
    [TabGroup("Skills")]
    [Title("Passive")]
    [SerializeField]
    private List<EquipmentPassiveSkill> passiveSkills;

    public IEnumerable<Skill> GetValidActiveSkill (SkillValidatorContext ctx)
        => activeSkills
            .Where(ws => ws.CanUseSkill(ctx))
            .Select(ws => ws.skill);

    public IEnumerable<SkillPassive> GetValidPassiveSkill (SkillValidatorContext ctx)
        => passiveSkills
            .Where(es => es.CanUseSkill(ctx))
            .Select(es => es.skill);

    public override object Clone()
    {
        var clone = (EquipmentData)base.Clone();
        
        clone.activeSkills = activeSkills.ToList();
        clone.passiveSkills = passiveSkills.ToList();
            
        return clone;
    }
}