using System;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class LevelRequirement
{
    [MinValue(1), MaxValue(8)]
    public int level;

    [MinValue(0)]
    public int amount;
}

// TODO: Pass it to a WINDOW
public class PersistentManager : SingletonScriptableObject<PersistentManager>
{
    [TabGroup("Experience grown")]
    [TableList(AlwaysExpanded = true)]
    [SerializeField]
    private List<LevelRequirement> levelRequirements;

    [TabGroup("Valid modifiers")]
    [TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
    public List<ModifierIcons> icons;

    public int GetLevel (int experienceAmount)
    {
        for (var i = 1; i < levelRequirements.Count; i++)
            if (experienceAmount < levelRequirements[i].amount)
                return levelRequirements[i - 1].level;

        return GameConstants.Character.MaxCharacterLevel;
    }

    public int GetExperienceNecessaryToLevel (int level)
        => levelRequirements
            .First(l => l.level == level)
            .amount;

    public Sprite GetIcon (ModifierKind kind)
    {
        var modifier = icons.FirstOrDefault(m => m.kind == kind);

        if (modifier == null)
            throw new Exception($"Didnt have any icon set for {kind}");

        return modifier.icon;
    }
}