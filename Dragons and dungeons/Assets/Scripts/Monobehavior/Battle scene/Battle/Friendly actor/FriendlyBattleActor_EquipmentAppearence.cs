using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class FriendlyBattleActor
{
#if UNITY_EDITOR
    [ShowInInspector]
    [HideInEditorMode]
    public Equipment currentEquipment => Data?.Equipment ?? null;
#endif

    [TabGroup("References")]
    [Title("Equipments")]
    [SerializeField]
    private Transform helmetFolder;

    [TabGroup("References")]
    [SerializeField]
    private Transform weaponFolder;

    [TabGroup("References")]
    [SerializeField]
    private Transform armorFolder;

    private GameObject cachedArmor;

    private GameObject cachedHelmet;
    private GameObject cachedWeapon;

    public void RedrawEquipment()
    {
        RedrawHelmet(Data.Equipment.headArmor);
        RedrawWeapon(Data.Equipment.weapon);
        RedrawArmor(Data.Equipment.chestArmor);
    }

    [Button]
    private void RedrawArmor (ChestArmor armor)
    {
        HideArmor();
        if (armor == null) // unequip
            return;

        cachedArmor = FindCorrectChild(armor.idOnRuntime, armorFolder);
        ShowArmor();

        if (IsGeneralEquipment(armor.idOnRuntime))
            cachedArmor.GetComponent<SpriteRenderer>().sprite = armor.GetIconSprite;
    }

    private void RedrawWeapon (Weapon weapon)
    {
        HideWeapon();
        if (weapon == null) // unequip
            return;

        cachedWeapon = FindCorrectChild(weapon.idOnRuntime, weaponFolder);
        ShowWeapon();

        if (IsGeneralEquipment(weapon.idOnRuntime))
            cachedWeapon.GetComponent<SpriteRenderer>().sprite = weapon.GetIconSprite;
    }

    private void RedrawHelmet (HeadArmor helmet)
    {
        HideHelmet();
        if (helmet == null) // unequip
            return;

        cachedHelmet = FindCorrectChild(helmet.idOnRuntime, helmetFolder);
        ShowHelmet();

        if (IsGeneralEquipment(helmet.idOnRuntime))
            cachedHelmet.GetComponent<SpriteRenderer>().sprite = helmet.GetIconSprite;
    }

    private void ShowArmor()
    {
        if (cachedArmor != null)
            cachedArmor.SetActive(true);
    }

    private void ShowWeapon()
    {
        if (cachedWeapon != null)
            cachedWeapon.SetActive(true);
    }

    private void ShowHelmet()
    {
        if (cachedHelmet != null)
            cachedHelmet.SetActive(true);
    }

    private void HideArmor()
    {
        if (cachedArmor != null)
            cachedArmor.SetActive(false);
    }

    private void HideWeapon()
    {
        if (cachedWeapon != null)
            cachedWeapon.SetActive(false);
    }

    private void HideHelmet()
    {
        if (cachedHelmet)
            cachedHelmet.SetActive(false);
    }

    private static bool IsGeneralEquipment (int id)
        => id == 0;

    private GameObject FindCorrectChild (int id, Transform folder)
        => (from Transform child in folder
            where child
                .name
                .StartsWith(id.ToString())
            select child.gameObject).FirstOrDefault();
}