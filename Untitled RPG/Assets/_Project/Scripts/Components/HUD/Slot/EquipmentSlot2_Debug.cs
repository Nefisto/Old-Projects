#if UNITY_EDITOR
public partial class EquipmentSlot2
{
    [DisableInEditorButton]
    private void T_SetupWeapon (WeaponDataFactory weaponDataFactory)
        => StartCoroutine(Setup(new Settings() { equipmentData = weaponDataFactory.GetInstance() }));

    [DisableInEditorButton]
    private void T_SetupArmor (ArmorDataFactory armorDataFactory)
        => StartCoroutine(Setup(new Settings() { equipmentData = armorDataFactory.GetInstance() }));
}
#endif