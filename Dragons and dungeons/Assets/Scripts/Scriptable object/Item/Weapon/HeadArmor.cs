using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.HeadArmorName, menuName = Nomenclature.HeadArmorMenu, order = 0)]
public class HeadArmor : EquipmentData
{
    protected override ItemData CreateNewInstance => CreateInstance<HeadArmor>();
}