using System;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[Serializable]
public partial class Template : IEquatable<Template>, IEntryPointContext
{
    [TitleGroup("Settings")]
    public TraitChart traitChart = new();

    [TitleGroup("Settings")]
    public CurrentEquipment currentEquipment = new();

    [TitleGroup("Settings")]
    public GameJob initialJob;

    [TitleGroup("Settings")]
    public List<Skill> skills = new();

    public SaveSlotIconEnum characterIconEnum;
    public Template GetInstance => (Template)SerializationUtility.CreateCopy(this);

    public void Setup() => currentEquipment.Setup(); // Load default gear

    public void LoadReferences()
    {
        var weaponName = currentEquipment.CurrentWeapon.Name;
        var weapon = Database.Weapons.GetInstanceDataThatMatch(gear => gear.Name == weaponName);
        currentEquipment.Equip(weapon);

        var armorName = currentEquipment.CurrentArmor.Name;
        var armor = Database.Armors.GetInstanceDataThatMatch(gear => gear.Name == armorName);
        currentEquipment.Equip(armor);

        skills = skills
            .Select(skill => Database.Skills.GetDataThatMatch(s => s.Name == skill.Name).GetInstance)
            .ToList();
    }
}