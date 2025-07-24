using System;
using System.Collections.Generic;
using System.Linq;

public abstract partial class Skill
{
    public IEnumerable<AttributeOnHUD> AttributesIterator()
        => AttributesInOrder()
            .Select(ToAttributeOnHUD);

    public IEnumerable<AttributeOnHUD> MainAttributesIterator()
        => MainAttributesOrderProvider
            .GetCorrectOrder(MainAttributesOrder)
            .Take(4)
            .Select(ToAttributeOnHUD);

    private IEnumerable<SkillAttribute> AttributesInOrder()
        => Enum
            .GetValues(typeof(SkillAttribute))
            .Cast<SkillAttribute>();

    private AttributeOnHUD ToAttributeOnHUD (SkillAttribute skillAttribute)
        => skillAttribute switch
        {
            SkillAttribute.ManaCost => new("Mana cost", ResourceCost == 0 ? "-" : ResourceCost.ToString()),
            SkillAttribute.Cooldown => new("Cooldown", Cooldown == 0f ? "-" : $"{Cooldown}s"),
            SkillAttribute.RecoveryTime => new("Fatigue", FatigueAmount == 0f ? "-" : $"{FatigueAmount}"),
            SkillAttribute.ChanceToHit => new("Chance to hit", ChanceToHit == 0f ? "-" : $"{ChanceToHit * 100:###}%"),
            SkillAttribute.CriticalChance => new("Critical chance",
                CriticalChance == 0f ? "-" : $"{CriticalChance * 100:###}%"),
            _ => throw new ArgumentOutOfRangeException(nameof(skillAttribute), skillAttribute, null)
        };


    public class AttributeOnHUD
    {
        public string label;
        public string value;

        public AttributeOnHUD (string label, string value)
        {
            this.label = label;
            this.value = value;
        }
    }
}