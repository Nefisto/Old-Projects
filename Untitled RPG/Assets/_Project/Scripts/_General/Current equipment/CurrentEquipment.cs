using System;
using UnityEngine;

[Serializable]
public partial class CurrentEquipment : IPotentialProvider, IEquatable<CurrentEquipment>
{
    [field: SerializeField]
    public WeaponData CurrentWeapon { get; private set; }

    [field: SerializeField]
    public ArmorData CurrentArmor { get; private set; }

    public int StrengthPotential => CurrentWeapon.StrengthPotential + CurrentArmor.StrengthPotential;
    public int VitalityPotential => CurrentWeapon.VitalityPotential + CurrentArmor.VitalityPotential;
    public int DexterityPotential => CurrentWeapon.DexterityPotential + CurrentArmor.DexterityPotential;
    public int IntelligencePotential => CurrentWeapon.IntelligencePotential + CurrentArmor.IntelligencePotential;

    public void Setup()
    {
        CurrentWeapon ??= Database.Weapons.Fallback.GetInstance();
        CurrentArmor ??= Database.Armors.Fallback.GetInstance();
    }

    public void Equip (EquipmentData equipmentData)
    {
        switch (equipmentData.EquipmentKind)
        {
            case EquipmentKind.Weapon:
                CurrentWeapon = equipmentData as WeaponData;
                break;

            case EquipmentKind.Armor:
                CurrentArmor = equipmentData as ArmorData;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}