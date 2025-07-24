using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.WeaponName, menuName = Nomenclature.WeaponMenu, order = 0)]
public class Weapon : EquipmentData
{
    protected override ItemData CreateNewInstance => CreateInstance<Weapon>();
}