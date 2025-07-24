using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.ChestArmorName, menuName = Nomenclature.ChestArmorMenu, order = 0)]
public class ChestArmor : EquipmentData
{
    protected override ItemData CreateNewInstance => CreateInstance<ChestArmor>();
}