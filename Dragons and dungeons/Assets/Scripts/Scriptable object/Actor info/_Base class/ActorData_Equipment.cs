using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class ActorData
{
    public event Action OnChangeEquipment;

    [TabGroup("Data tab", "Equipment")]
    [PropertyOrder(10)]
    [Title("Initial equipment")]
    [HideLabel]
    [SerializeField]
    private Equipment equipment = new Equipment();

    public Equipment Equipment => equipment;

    public void EquipWeapon (Weapon newWeapon)
    {
        Equipment.weapon = newWeapon;

        OnChangeEquipment?.Invoke();
    }

    public void EquipHead (HeadArmor newHeadArmor)
    {
        Equipment.headArmor = newHeadArmor;

        OnChangeEquipment?.Invoke();
    }

    public void EquipChestArmor (ChestArmor chestArmor)
    {
        Equipment.chestArmor = chestArmor;

        OnChangeEquipment?.Invoke();
    }

    public IEnumerable<Skill> GetValidActiveSkills()
    {
        var validSkill = defaultSkills.ToList();

        if (!HasWeaponEquipped())
            return validSkill;

        var weaponActiveSkills = Equipment.weapon
            .GetValidActiveSkill(new SkillValidatorContext() { status = GetBaseStatus() });
        validSkill.AddRange(weaponActiveSkills);

        return validSkill;
    }

    private IEnumerable<SkillPassive> GetWeaponPassiveSkills()
    {
        if (!HasWeaponEquipped())
            yield break;

        var weaponPassiveSkills = Equipment.weapon
            .GetValidPassiveSkill(new SkillValidatorContext { status = GetBaseStatus() });

        foreach (var weaponPassiveSkill in weaponPassiveSkills)
            yield return weaponPassiveSkill;
    }

    private bool HasWeaponEquipped()
        => Equipment.weapon != null;

    private void EquipmentClone (ActorData clone)
        => clone.equipment = new Equipment(equipment);
}